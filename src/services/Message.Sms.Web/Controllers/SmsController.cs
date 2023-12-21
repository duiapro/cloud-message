using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.OpenSDK.ApiService;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var user = await _dbContext.Users.FindAsync(usersSmsCodeLogs.UserId);
            _ = user ?? throw new Exception("user non-existent");

            var channel =
                await _dbContext.Channels.FirstOrDefaultAsync(channel => channel.KeyId == usersSmsCodeLogs.ChannelId);
            var discountPrice = await this.VerifyUserBalanceAsync(user.KeyId, channel.KeyId);
            var phone = await _smsApiClientAdapter.GetPhoneCodeAsync(usersSmsCodeLogs.ApiServiceProviderType,
                usersSmsCodeLogs.ApiServiceProviderType switch
                {
                    nameof(ApocalypseSmsApiClient) => new ApocalypseGetPhoneCodeRequest(channel.Code,
                        usersSmsCodeLogs.Mobile),
                    //todo:..
                    _ => throw new Exception("channelKeyId not querying valid")
                });

            usersSmsCodeLogs.SetCode(phone.Code, phone.Context);

            if (!string.IsNullOrEmpty(usersSmsCodeLogs.Context))
            {
                user.Balance -= discountPrice;

                _dbContext.Update(user);
                _dbContext.Update(usersSmsCodeLogs);
            }

            return Content(usersSmsCodeLogs.Context);
        }

        private async Task<decimal> VerifyUserBalanceAsync(Guid userKeyId, Guid channelKeyId)
        {
            var user = await _dbContext.Users.FindAsync(userKeyId);
            if (user.IsAdmin) return 0; //admin user no verification balance
            var channel = await _dbContext.Channels.FindAsync(channelKeyId);
            var discountPrice = (channel.Price * user.Discount);
            if ((user.Balance - discountPrice) < 0)
                throw new Exception("User verification balance is insufficient, please recharge.");

            return discountPrice;
        }
    }
}