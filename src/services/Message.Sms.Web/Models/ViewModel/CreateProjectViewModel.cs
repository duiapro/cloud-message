using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Models.ViewModel
{
    public class CreateProjectViewModel
    {
        [Required]
        public Guid ApiServiceProviderId { get; set; }

        [Required, MaxLength(20)]
        public string ProjectName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string SubTitle { get; set; } = string.Empty;

        public int Sort { get; set; }=0;

        public int Grade { get; set; } = 1;

        public IFormFile? Icon { get; set; }

        public bool Status { get; set; } = true;

        public string Description { get; set; } = string.Empty;
    }
}
