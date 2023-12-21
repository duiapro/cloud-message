namespace Message.Sms.Web.OpenSDK.Models
{
    public class PhoneCodeResponse
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Context { get; set; }

        public PhoneCodeResponse(string? code, string? context, string? message = "")
        {
            Code = code;
            Message = message;
            Context = context;
        }
    }
}
