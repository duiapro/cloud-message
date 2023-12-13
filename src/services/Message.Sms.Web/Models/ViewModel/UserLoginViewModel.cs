using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Models.ViewModel
{
    public class UserLoginViewModel
    {
        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }

        [DefaultValue(true)]
        public bool Rememberme { get; set; }
    }
}
