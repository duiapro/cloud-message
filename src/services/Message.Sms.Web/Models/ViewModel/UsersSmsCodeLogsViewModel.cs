using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Models.ViewModel
{
    public class UsersSmsCodeLogsViewModel
    {
        public Guid KeyId { get; set; }

        public Guid UserId { get; set; }

        public Guid ChannelId { get; set; }

        public string ChannelName { get; set; }

        public string ChannelCode { get; set; }

        public decimal Price { get; set; }

        public string ApiServiceProviderType { get; set; }

        public string Mobile { get; set; }

        public string Code { get; set; }

        public string Context { get; set; }

        //1.start 2.runing 3.end
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
