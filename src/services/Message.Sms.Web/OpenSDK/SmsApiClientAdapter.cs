using Message.Sms.Web.OpenSDK.ApiService;

namespace Message.Sms.Web.OpenSDK;

public class SmsApiClientAdapter
{
    private readonly IServiceProvider serviceProvider;

    public SmsApiClientAdapter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public SmsApiClientBase? Get<T>(string type) where T : SmsApiClientBase
    {
        switch (type)
        {
            case nameof(ApocalypseSmsApiClient):
                return serviceProvider.GetRequiredService<T>();
            default:
                return null;
        }
    }

}