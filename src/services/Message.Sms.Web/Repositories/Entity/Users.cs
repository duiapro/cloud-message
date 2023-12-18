using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users")]
    public class Users : EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(15)]
        public string UserMobile { get; set; }

        [Required]
        [MaxLength(15)]
        public string PassWork { get; set; }

        public decimal Balance { get; set; }

        public bool IsVip { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; } = 1;

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取日志
        /// </summary>
        private List<UsersSmsCodeLogs> _codeLogs;
        public IReadOnlyCollection<UsersSmsCodeLogs>? CodeLogs => _codeLogs?.AsReadOnly();

        public Users() { }

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
    }
}
