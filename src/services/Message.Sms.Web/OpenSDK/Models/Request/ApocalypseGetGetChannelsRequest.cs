using System.Diagnostics.Contracts;

namespace Message.Sms.Web.OpenSDK.Models.Request
{
    public class ApocalypseGetGetChannelsRequest : RequestBase
    {
        public int keyword { get; set; } = 0;

        public List<ApocalypseProjectChannelIdList> channelIdList { get; set; }

        public bool priceSort { get; set; } = false;

        public int page { get; set; } = 1;

        public ApocalypseGetGetChannelsRequest(List<ApocalypseProjectChannelIdList> channelIdList)
        {
            this.channelIdList = channelIdList;
            this.keyword = channelIdList.GetHashCode();
        }

        public ApocalypseGetGetChannelsRequest()
        {
        }
    }
}