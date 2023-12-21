namespace Message.Sms.Web.Models.ViewModel
{
    public class CreateRechargeCardViewModel
    {
        public int Number { get; set; }

        public decimal Amount { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Remark { get; set; } = string.Empty;
    }
}
