using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.OpenSDK.ApiService;
using Message.Sms.Web.OpenSDK.Models;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Threading.Channels;

namespace Message.Sms.Web.Controllers
{
    [AuthFilter]
    public class SmsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly SmsApiClientAdapter _smsApiClientAdapter;

        public SmsController(AppDbContext dbContext, SmsApiClientAdapter smsApiClientAdapter)
        {
            _dbContext = dbContext;
            _smsApiClientAdapter = smsApiClientAdapter;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPhoneNumber(Guid channelKeyId)
        {
            var channel = await _dbContext.Channels.Include(channel => channel.ApiServiceProvider)
                .FirstOrDefaultAsync(channel => channel.KeyId == channelKeyId);

            _ = channel ?? throw new Exception("channelKeyId not querying valid.");
            var loginUserKeyId = base.AppUsers.KeyId ?? throw new Exception("user not login.");

            var thisDateTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
            var thisGetPhoneNumberCount = await _dbContext.UsersSmsCodeLogs.CountAsync(logs =>
                thisDateTime > logs.CreateTime && logs.Context == string.Empty && logs.UserId == loginUserKeyId);
            if (thisGetPhoneNumberCount > 100)
                throw new Exception("Today’s mobile phone number acquisition limit has been reached.");

            await this.VerifyUserBalanceAsync(loginUserKeyId, channel.KeyId);

            var phone = await _smsApiClientAdapter.GetPhoneAsync(channel.ApiServiceProvider.Type,
                channel.ApiServiceProvider.Type switch
                {
                    nameof(ApocalypseSmsApiClient) => new ApocalypseGetPhoneRequest(channelId: channel.Code),
                    //todo:..
                    _ => throw new Exception("channelKeyId not querying valid.")
                });
            await _dbContext.UsersUseMobileHistorys.AddAsync(new(loginUserKeyId, channelKeyId,
                channel.ApiServiceProvider.Type, channel.Name, phone.Mobile));
            var usersSmsCodeLogs = await _dbContext.UsersSmsCodeLogs.AddAsync(new(loginUserKeyId, channelKeyId,
                channel.ApiServiceProvider.Type, phone.Mobile));
            await _dbContext.SaveChangesAsync();

            return Json(new { Mobile = phone.Mobile, TaskKeyId = usersSmsCodeLogs.Entity.KeyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetCode(Guid taskKeyId)
        {
            var loginUser = base.AppUsers;
            var usersSmsCodeLogs =
                await _dbContext.UsersSmsCodeLogs.FirstOrDefaultAsync(logs =>
                    logs.KeyId == taskKeyId && logs.UserId == loginUser.KeyId);
            _ = usersSmsCodeLogs ?? throw new Exception("taskKeyId not querying valid");

            #region one test code

            //var user = await _dbContext.Users.FindAsync(usersSmsCodeLogs.UserId);
            //_ = user ?? throw new Exception("user non-existent");

            //var channel =
            //    await _dbContext.Channels.FirstOrDefaultAsync(channel => channel.KeyId == usersSmsCodeLogs.ChannelId);
            //var discountPrice = await this.VerifyUserBalanceAsync(user.KeyId, channel.KeyId);
            //var phone = await _smsApiClientAdapter.GetPhoneCodeAsync(usersSmsCodeLogs.ApiServiceProviderType,
            //    usersSmsCodeLogs.ApiServiceProviderType switch
            //    {
            //        nameof(ApocalypseSmsApiClient) => new ApocalypseGetPhoneCodeRequest(channel.Code,
            //            usersSmsCodeLogs.Mobile),
            //        //todo:..
            //        _ => throw new Exception("channelKeyId not querying valid")
            //    });

            //if (true)//!string.IsNullOrEmpty(usersSmsCodeLogs.Context)
            //{
            //    var (balanceBill, Balance) = user.DeductBalance(discountPrice, "获取验证码", $"{channel.Name}【{usersSmsCodeLogs.Mobile}】获取用验证码成功");
            //    usersSmsCodeLogs.SetCode(phone.Code, phone.Context);

            //    _dbContext.Update(user);
            //    _dbContext.Add(balanceBill);
            //    _dbContext.Update(usersSmsCodeLogs);
            //    await _dbContext.SaveChangesAsync();
            //}

            #endregion

            return Json(new
            {
                Status = usersSmsCodeLogs.Status,
                Context = usersSmsCodeLogs.Status == 3 && string.IsNullOrEmpty(usersSmsCodeLogs.Context)
                    ? "Failed to obtain verification code, balance has been returned."
                    : usersSmsCodeLogs.Context
            });
        }

        [HttpPost]
        public async Task StartGetCodeThread([FromQuery] Guid taskKeyId)
        {
            var loginUser = base.AppUsers;
            var usersSmsCodeLogs =
                await _dbContext.UsersSmsCodeLogs.FirstOrDefaultAsync(logs =>
                    logs.KeyId == taskKeyId && logs.UserId == loginUser.KeyId);
            _ = usersSmsCodeLogs ?? throw new Exception("taskKeyId not querying valid");

            var user = await _dbContext.Users.FindAsync(usersSmsCodeLogs.UserId);
            _ = user ?? throw new Exception("user non-existent");

            if (usersSmsCodeLogs.Status != 1)
            {
                return;
            }

            var channel =
                await _dbContext.Channels.FirstOrDefaultAsync(channel => channel.KeyId == usersSmsCodeLogs.ChannelId);
            var discountPrice = await this.VerifyUserBalanceAsync(user.KeyId, channel.KeyId);

            var threadParameter = new
            {
                ApiServiceProviderType = usersSmsCodeLogs.ApiServiceProviderType,
                UserKeyId = user.KeyId,
                TaskKeyId = taskKeyId,
                ChannelKeyId = channel.KeyId,
                ChannelCode = channel.Code,
                ChannelName = channel.Name,
                Mobile = usersSmsCodeLogs.Mobile
            };

            Thread thread = new(new ParameterizedThreadStart(GetCodeAsyncThread));
            thread.Start(threadParameter);

            usersSmsCodeLogs.StartRuning();

            var (balanceBill, Balance) = user.DeductBalance(discountPrice, "Get Code",
                $"{channel.Name}【{usersSmsCodeLogs.Mobile}】- Get the balance withheld with verification code");

            _dbContext.Update(user);
            _dbContext.Update(usersSmsCodeLogs);
            await _dbContext.SaveChangesAsync();
        }

        private void GetCodeAsyncThread(dynamic parameter)
        {
            try
            {
                int count = 200;
                int interval = 1200;

                string apiServiceProviderType = parameter.ApiServiceProviderType;
                Guid userKeyId = parameter.UserKeyId;
                Guid taskKeyId = parameter.TaskKeyId;
                Guid channelKeyId = parameter.ChannelKeyId;
                string channelCode = parameter.ChannelCode;
                string channelName = parameter.ChannelName;
                string mobile = parameter.Mobile;

                PhoneCodeResponse? phoneCode = null;
                for (int i = 0; i < count; i++)
                {
                    Console.Out.WriteLineAsync($"{DateTime.Now} -- start run num {i}");
                    Thread.Sleep(interval);
                    phoneCode = Task.Run(async () =>
                    {
                        var phone = await _smsApiClientAdapter.GetPhoneCodeAsync(apiServiceProviderType,
                            apiServiceProviderType switch
                            {
                                nameof(ApocalypseSmsApiClient) =>
                                    new ApocalypseGetPhoneCodeRequest(channelCode, mobile),
                                //todo:..
                                _ => throw new Exception("channelKeyId not querying valid")
                            });
                        return phone;
                    }).Result;

                    if (!string.IsNullOrEmpty(phoneCode?.Code)) break;
                }

                Console.Out.WriteLineAsync($"{DateTime.Now} -- end run result [{phoneCode?.Code}]");
                using (var dbContext = DbContextFactory.Create())
                {
                    var user = dbContext.Users.Find(userKeyId);
                    var channel = dbContext.Channels.Find(channelKeyId);
                    var usersSmsCodeLogs =
                        dbContext.UsersSmsCodeLogs.FirstOrDefault(logs =>
                            logs.KeyId == taskKeyId && logs.UserId == userKeyId);

                    if (user is null) return;
                    if (channel is null) return;
                    if (usersSmsCodeLogs is null) return;

                    var discountPrice = (channel.Price * user.Discount);
                    usersSmsCodeLogs.SetCode(phoneCode?.Code, phoneCode?.Context);
                    if (string.IsNullOrEmpty(phoneCode?.Code))
                    {
                        Console.Out.WriteLineAsync($"{DateTime.Now} -- RechargeBalance [{discountPrice}]");
                        user.RechargeBalance(discountPrice, "Get Code",
                            "Failed to obtain verification code, balance returned.");
                    }

                    dbContext.Update(user);
                    dbContext.Update(usersSmsCodeLogs);

                    dbContext.SaveChanges();
                    Console.Out.WriteLineAsync($"{DateTime.Now} -- SaveChangesAsync");
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLineAsync(ex.Message);
            }
        }

        private async Task<decimal> VerifyUserBalanceAsync(Guid userKeyId, Guid channelKeyId)
        {
            var user = await _dbContext.Users.FindAsync(userKeyId);
            //if (user.IsAdmin) return 0; //admin user no verification balance
            var channel = await _dbContext.Channels.FindAsync(channelKeyId);
            var discountPrice = (channel.Price * user.Discount);
            if ((user.Balance - discountPrice) < 0)
                throw new Exception("User verification balance is insufficient, please recharge.");

            return discountPrice;
        }
    }
}