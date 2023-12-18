using Message.Sms.Web.OpenSDK.ApiService;

namespace Message.Sms.Web.OpenSDK
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsApiClient(this IServiceCollection services, string serviceUrl = "")
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                serviceUrl = configuration?.GetValue<string>("ApocalypseSmsApiHost")!;
                if (string.IsNullOrWhiteSpace(serviceUrl))
                {
                    throw new ArgumentNullException(nameof(serviceUrl));
                }
            }

            services.AddHttpClient(nameof(ApocalypseSmsApiClient),
                httpclient => { httpclient.BaseAddress = new Uri(serviceUrl); });
            services.AddSingleton<ApiClientTokenManage>();
            services.AddSingleton<ISmsApiClient, ApocalypseSmsApiClient>();
            services.AddSingleton<SmsApiClientAdapter>();

            return services;
        }
    }
}