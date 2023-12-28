
using Message.Sms.Web.Repositories;
using Microsoft.EntityFrameworkCore;

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
            using var dbContext = DbContextFactory.Create();

            while (!stoppingToken.IsCancellationRequested)
            {
                var usersSmsCodeLogs = await dbContext.UsersSmsCodeLogs.Where(item => item.Status == 3).AsNoTracking().ToListAsync();

                Console.WriteLine($"{DateTime.Now}");

                await Task.Delay(500);
            }
        }
    }
}
