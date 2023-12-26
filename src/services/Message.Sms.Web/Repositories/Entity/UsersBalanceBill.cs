using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users_balance_bill")]
    public class UsersBalanceBill : EntityBase
    {
        public Guid UserKeyId { get; set; }

        public string Title { get; set; } = string.Empty;

        //1.top up 2.deduct
        public int Type { get; set; }

        public decimal Amount { get; set; }

        public decimal BeforeBalance { get; set; }

        public decimal AfterBalance { get; set; }

        public string Remake { get; set; } = string.Empty;

        public virtual Users Users { get; set; }

        public UsersBalanceBill()
        {
        }

        public UsersBalanceBill(Guid userKeyId, string title, int type, decimal amount, decimal beforeBalance, decimal afterBalance, string remake)
        {
            UserKeyId = userKeyId;
            Title = title;
            Type = type;
            Amount = amount;
            BeforeBalance = beforeBalance;
            AfterBalance = afterBalance;
            Remake = remake;
        }
    }
}
