using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("recharge_card")]
    public class RechargeCard : EntityBase
    {
        public Guid Code { get; set; }

        public decimal Amount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsActive { get; set; }

        public string Remark { get; set; } = string.Empty;

        public RechargeCard(Guid code, decimal amount, DateTime? startTime, DateTime? endTime, string? remark = "")
        {
            Code = code;
            Amount = amount;
            StartTime = startTime ?? DateTime.MinValue;
            EndTime = endTime ?? DateTime.MinValue;
            Remark = remark ?? string.Empty;
        }

        public RechargeCard()
        {
        }

        public void Verify()
        {
            if (this.IsActive)
            {
                throw new Exception("cardPassword used! ");
            }

            if (this.StartTime > DateTime.Now)
            {
                throw new Exception("cardPassword Not officially effective! ");
            }

            if (this.EndTime < DateTime.Now)
            {
                throw new Exception("cardPassword expired! ");
            }
        }

        public void VerifyAndUse()
        {
            this.Verify();
            this.SetActive();
        }

        public void SetActive()
        {
            IsActive = true;
        }
    }
}