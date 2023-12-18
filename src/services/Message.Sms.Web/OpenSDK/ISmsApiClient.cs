using Message.Sms.Web.OpenSDK.Models;

namespace Message.Sms.Web.OpenSDK;

public interface ISmsApiClient
{
    Task<LoginResponse> LoginAsync(RequestBase? request = null);

    Task<WalletResponse> GetWalletAsync(RequestBase? request = null);
    
    Task<PhoneResponse> GetPhoneAsync(RequestBase? request = null);
    
    Task<dynamic> GetPhoneCodeAsync(RequestBase? request = null);
}