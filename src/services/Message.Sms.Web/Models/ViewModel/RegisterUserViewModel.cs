using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Models.ViewModel
{
    public class RegisterUserViewModel
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
        [MinLength(6, ErrorMessage = "passwprd is min length 6")]
        public string PassWork { get; set; }

        [DefaultValue(true)]
        public bool IsAgreement { get; set; }
    }
}
