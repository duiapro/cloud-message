using Message.Sms.Web.OpenSDK.Models;

namespace Message.Sms.Web.OpenSDK;

public interface ISmsApiClient
{
    Task<LoginResponse> LoginAsync(RequestBase? request = null);

    Task<WalletResponse> GetWalletAsync(RequestBase? request = null);

    Task<PhoneResponse> GetPhoneAsync(RequestBase? request = null);

    Task<PhoneCodeResponse> GetPhoneCodeAsync(RequestBase? request = null);

    Task<T> GetProjectAsync<T>(string projectName);

    Task<T> GetChannelAsync<T>(RequestBase? request = null);

    Task<ApocalypseGetChannelIdResposne> GetChannelIdAsync(RequestBase? request = null);
}