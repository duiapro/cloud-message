using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Models.ViewModel
{
    public class CreateOpenSDKAccountViewModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string PassWord { get; set; }

        [Required]
        public string Remark { get; set; } = string.Empty;

        public string Authority { get; set; } = string.Empty;
    }
}
