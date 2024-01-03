using Message.Sms.Web.OpenSDK.ApiService;
using Message.Sms.Web.OpenSDK.Models;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories.Entity;

namespace Message.Sms.Web.OpenSDK;

public class SmsApiClientAdapter : ISmsApiClientAdapter
{
    public Guid Id { get; set; } = Guid.NewGuid();

    private readonly IEnumerable<ISmsApiClient> _smsApiClients;

    public SmsApiClientAdapter(IEnumerable<ISmsApiClient> serviceProvider)
    {
        this._smsApiClients = serviceProvider;
    }

    public ISmsApiClient Get(string type) => _smsApiClients.FirstOrDefault(client => client.GetType().Name == type) ??
                                             throw new ArgumentException(
                                                 $"The channel({type}) is abnormal, please contact customer service.");

    public ISmsApiClient GetApocalypseSmsApiClient => Get(Message.Sms.Web.OpenSDK.ApiService.ApocalypseSmsApiClient.GetServiceType);

    public async Task<PhoneCodeResponse> GetPhoneCodeAsync(string type, RequestBase? request = null)
        => await Get(type).GetPhoneCodeAsync(request);

    public async Task<LoginResponse> LoginAsync(string type, RequestBase? request = null)
        => await Get(type).LoginAsync(request);

    public async Task<WalletResponse> GetWalletAsync(string type, RequestBase? request = null)
        => await Get(type).GetWalletAsync(request);

    public async Task<PhoneResponse> GetPhoneAsync(string type, RequestBase? request = null)
        => await Get(type).GetPhoneAsync(request);

    public async Task<T> GetProjectAsync<T>(string type, string projectName) => await Get(type).GetProjectAsync<T>(projectName);

    public async Task<T> GetChannelAsync<T>(string type, RequestBase? request) => await Get(type).GetChannelAsync<T>(request);

    public async Task<ApocalypseGetChannelIdResposne> GetChannelIdAsync(string type, RequestBase? request) => await Get(type).GetChannelIdAsync(request);
}

public interface ISmsApiClientAdapter
{
    Guid Id { get; set; }
    Task<PhoneCodeResponse> GetPhoneCodeAsync(string type, RequestBase? request = null);
}