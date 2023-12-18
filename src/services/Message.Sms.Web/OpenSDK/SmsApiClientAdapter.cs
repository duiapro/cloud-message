using Message.Sms.Web.OpenSDK.ApiService;

namespace Message.Sms.Web.OpenSDK;

public class SmsApiClientAdapter
{
    private readonly IEnumerable<ISmsApiClient> _smsApiClients;

    public SmsApiClientAdapter(IEnumerable<ISmsApiClient> serviceProvider)
    {
        this._smsApiClients = serviceProvider;
    }

    public ISmsApiClient Get(string type) => _smsApiClients.FirstOrDefault(client => client.GetType().Name == type) ??
                                             throw new ArgumentException(
                                                 $"The channel({type}) is abnormal, please contact customer service.");

    public ISmsApiClient GetApocalypseSmsApiClient => Get(Message.Sms.Web.OpenSDK.ApiService.ApocalypseSmsApiClient.GetServiceType);
}