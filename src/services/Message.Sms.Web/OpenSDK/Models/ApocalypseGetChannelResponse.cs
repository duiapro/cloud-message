namespace Message.Sms.Web.OpenSDK.Models
{
    public class ApocalypseGetChannelResponse
    {
        public List<ApocalypseGetChanneData> List { get; set; }
    }

    public class ApocalypseGetChanneData : RequestBase
    {
        public int? canUseMum { get; set; }

        public double? cardMoney { get; set; }

        public int? cardUid { get; set; }

        public int? channelRatio { get; set; }

        public string? code { get; set; }

        public string? content { get; set; }

        public string? first { get; set; }

        public int? id { get; set; }

        public int? inNum { get; set; }

        public int? isIn { get; set; }

        public int? isMine { get; set; }

        public string? lastSuccessTime { get; set; }

        public string? numberSegment { get; set; }

        public int? onlineMum { get; set; }

        public int? platform { get; set; }

        public string? potatoUrl { get; set; }

        public int? projectId { get; set; }

        public string? projectName { get; set; }

        public string? province { get; set; } = string.Empty;

        public string? supplier { get; set; }

        public string? supplierId { get; set; }

        public string? type { get; set; }

        public double? userMoney { get; set; }
    }
}
