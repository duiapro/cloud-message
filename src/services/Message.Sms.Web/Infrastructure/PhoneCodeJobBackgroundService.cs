using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace Message.Sms.Web.Infrastructure
{
    public class PhoneCodeJobBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public PhoneCodeJobBackgroundService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();

            var smsApiClientAdapter = scope.ServiceProvider.GetRequiredService<SmsApiClientAdapter>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var dbContext = DbContextFactory.Create())
                    {
                        await Console.Out.WriteLineAsync($"dbContext:{dbContext.ContextId.ToString()}");
                        var usersSmsCodeLogs = await dbContext.UsersSmsCodeLogs.Where(item => item.Status != 3)
                        .AsNoTracking()
                        .ToListAsync(stoppingToken);
                        var updates = new List<UsersSmsCodeLogs>();

                        await Parallel.ForEachAsync(usersSmsCodeLogs, new ParallelOptions() { MaxDegreeOfParallelism = 1 },
                            async (smsCodeLog, cancellationToken) =>
                            {
                                var user = dbContext.Users.Find(smsCodeLog.UserId);

                                if (smsCodeLog.CreateTime < DateTime.Now.AddMinutes(-5))
                                {
                                    smsCodeLog.SetCode(string.Empty, string.Empty);
                                    updates.Add(smsCodeLog);

                                    await Console.Out.WriteLineAsync($"{DateTime.Now} -- RechargeBalance [{smsCodeLog.Price}]");
                                    user.RechargeBalance(smsCodeLog.Price, "Get Code Returned",
                                        "Failed to obtain verification code, balance returned.");
                                    dbContext.Update(user);
                                }
                                else
                                {
                                    bool isUpdate = false;
                                    if (smsCodeLog.Status == 1)
                                    {
                                        var (balanceBill, Balance) = user.DeductBalance(smsCodeLog.Price, "Get Code",
                                                            $"{smsCodeLog.ChannelCode}【{smsCodeLog.Mobile}】- Get the balance withheld with verification code");

                                        dbContext.Update(user);

                                        smsCodeLog.StartRuning();
                                        isUpdate = true;
                                    }
                                    var phone = await smsApiClientAdapter.GetPhoneCodeAsync(
                                        smsCodeLog.ApiServiceProviderType,
                                        smsCodeLog.ApiServiceProviderType switch
                                        {
                                            "ApocalypseSmsApiClient" =>
                                                new ApocalypseGetPhoneCodeRequest(smsCodeLog.ChannelCode,
                                                    smsCodeLog.Mobile),
                                            _ => null
                                        });
                                    if (!string.IsNullOrEmpty(phone?.Code))
                                    {
                                        smsCodeLog.SetCode(phone.Code, phone.Context);
                                        isUpdate = true;
                                    }
                                    if (isUpdate)
                                        updates.Add(smsCodeLog);
                                }
                            });

                        dbContext.UpdateRange(updates);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        if (updates.Any())
                        {
                            await Task.Delay(5000, stoppingToken);
                        }
                        else
                        {
                            await Task.Delay(15000, stoppingToken);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}