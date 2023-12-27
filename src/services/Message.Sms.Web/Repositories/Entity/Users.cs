using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users")]
    public class Users : EntityBase
    {
        [Required] [MaxLength(50)] public string UserName { get; set; }

        [Required] [MaxLength(15)] public string UserMobile { get; set; }

        [Required] [MaxLength(15)] public string PassWork { get; set; }

        public decimal Balance { get; set; }

        public bool IsVip { get; set; }

        [Precision(18, 2)] public decimal Discount { get; set; } = 1;

        public bool IsAdmin { get; set; }

        private List<UsersSmsCodeLogs> _codeLogs = new();
        public IReadOnlyCollection<UsersSmsCodeLogs>? CodeLogs => _codeLogs?.AsReadOnly();

        private List<UsersBalanceBill> _balanceBill = new();
        public IReadOnlyCollection<UsersBalanceBill>? BalanceBill => _balanceBill?.AsReadOnly();

        public Users()
        {
        }

        public Users(string userName, string userMobile, string passWork, bool isAdmin = false)
        {
            UserName = userName;
            UserMobile = userMobile;
            PassWork = passWork;
            Balance = 0;
            IsVip = false;
            Discount = 1;
            IsAdmin = isAdmin;
        }

        public (UsersBalanceBill, decimal) DeductBalance(decimal price, string title = "", string remake = "")
        {
            decimal beforeBalance = Balance;
            Balance -= price;
            decimal afterBalance = Balance;
            var balanceBill = new UsersBalanceBill(this.KeyId, title, 2, price, beforeBalance, afterBalance, remake);
            _balanceBill.Add(balanceBill);
            return (balanceBill, Balance);
        }

        public (UsersBalanceBill, decimal) RechargeBalance(decimal price, string title = "", string remake = "")
        {
            decimal beforeBalance = Balance;
            Balance += price;
            decimal afterBalance = Balance;
            var balanceBill = new UsersBalanceBill(this.KeyId, title, 1, price, beforeBalance, afterBalance, remake);
            _balanceBill.Add(balanceBill);
            return (balanceBill, Balance);
        }

        public decimal DeductDiscountBalance(decimal salesAmount)
        {
            if (IsAdmin)
                return this.DeductBalance(0).Item2; //admin user no verification balance

            var discountPrice = (salesAmount * Discount);
            if ((Balance - discountPrice) < 0)
                throw new Exception("User verification balance is insufficient, please recharge.");

            return this.DeductBalance(discountPrice).Item2;
        }
    }
}