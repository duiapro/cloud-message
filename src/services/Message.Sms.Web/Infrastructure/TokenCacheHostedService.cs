
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Infrastructure
{
    public class TokenCacheHostedService : IHostedService
    {
        private readonly ApiClientTokenManage apiClientTokenManage;
        private readonly IServiceScopeFactory scopeFactory;

        public TokenCacheHostedService(ApiClientTokenManage apiClientTokenManage, IServiceScopeFactory scopeFactory)
        {
            this.apiClientTokenManage = apiClientTokenManage;
            this.scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var apiAuthoritys = await dbContext.ApiServiceProviders.ToListAsync();
                foreach (var item in apiAuthoritys)
                {
                    await Console.Out.WriteLineAsync($"add token: {item.Type} value:{item.Authority}");
                    apiClientTokenManage.SetToken(item.Type, item.Authority);
                }
                await dbContext.DisposeAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
