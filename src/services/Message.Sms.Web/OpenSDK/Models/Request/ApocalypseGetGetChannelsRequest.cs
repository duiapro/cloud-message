using System.Diagnostics.Contracts;

namespace Message.Sms.Web.OpenSDK.Models.Request
{
    public class ApocalypseGetGetChannelsRequest
    {
        public int keyword { get; set; } = 0;

        public string channelIdList { get; set; }

        public bool priceSort { get; set; } = false;

        public int page { get; set; } = 1;

        public ApocalypseGetGetChannelsRequest(string channelIdList)
        {
            this.channelIdList = channelIdList;
        }
    }
}
