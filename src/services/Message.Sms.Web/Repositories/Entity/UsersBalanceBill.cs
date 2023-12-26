using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users_balance_bill")]
    public class UsersBalanceBill : EntityBase
    {
        public Guid Code { get; set; }

        public Guid UsersRechargeKeyId { get; set; }

        public decimal Amount { get; set; }

        public decimal BeforeBalance { get; set; }

        public decimal AfterBalance { get; set; }
    }
}
