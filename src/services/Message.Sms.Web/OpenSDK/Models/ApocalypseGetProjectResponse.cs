namespace Message.Sms.Web.OpenSDK.Models
{
    public class ApocalypseGetProjectResponse
    {
        public List<ApocalypseGetProjectData> List { get; set; }
    }

    public class ApocalypseProjectChannelIdList
    {
        public int id { get; set; }

        public string projectName { get; set; }

        public string supplier { get; set; }
    }

    public class ApocalypseGetProjectData
    {
        public List<ApocalypseProjectChannelIdList> channelIdList { get; set; }

        public int channelSize { get; set; }

        public int id { get; set; }

        public string myType { get; set; }

        public string name { get; set; }

        public int price { get; set; }

        public int selected { get; set; }

        public int sort { get; set; }

        public string supplier { get; set; }

        public string userPrice { get; set; }
    }
}
