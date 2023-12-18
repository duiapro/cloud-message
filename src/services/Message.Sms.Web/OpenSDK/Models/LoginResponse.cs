namespace Message.Sms.Web.OpenSDK.Models;

public class LoginResponse
{
    public LoginResponse(string apiKey)
    {
        ApiKey = apiKey;
    }

    public string ApiKey { get; set; }
}