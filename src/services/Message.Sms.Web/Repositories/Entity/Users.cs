using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users")]
    public class Users : EntityBase
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string UserMobile { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string PassWork { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 是否是VIP
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
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
