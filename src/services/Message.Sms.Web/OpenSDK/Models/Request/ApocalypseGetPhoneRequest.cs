namespace Message.Sms.Web.OpenSDK.Models.Request
{
    public class ApocalypseGetPhoneRequest : RequestBase
    {
        public string ChannelId { get; set; } = string.Empty;
        public string PhoneNum { get; set; } = string.Empty;
        public string Operators { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
    }
}
