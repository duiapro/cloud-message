namespace Message.Sms.Web.OpenSDK.Models.Request
{
    public class ApocalypseGetPhoneCodeRequest : RequestBase
    {
        public string ChannelId { get; set; } = string.Empty;
        public string PhoneNum { get; set; } = string.Empty;

        public ApocalypseGetPhoneCodeRequest()
        {
        }

        public ApocalypseGetPhoneCodeRequest(string channelId, string phoneNum)
        {
            ChannelId = channelId;
            PhoneNum = phoneNum;
        }
    }
}
