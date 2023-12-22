using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("project")]
    public class Project : EntityBase
    {
        [Required, MaxLength(20)]
        public string ProjectName { get; set; }

        [Required]
        public Guid ApiServiceProviderId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ApiServiceProviderType { get; set; }

        [Required, MaxLength(50)]
        public string SubTitle { get; set; }

        public int Sort { get; set; }

        public int Grade { get; set; }

        [Required, MaxLength(100)]
        public string Icon { get; set; }

        public bool Status { get; set; } = true;

        public string Description { get; set; } = string.Empty;

        public Project()
        {
        }

        public Project(Guid apiServiceProviderId, string apiServiceProviderType, string projectName, string subTitle, int sort, int grade, string icon, string description)
        {
            ApiServiceProviderType = apiServiceProviderType;
            ApiServiceProviderId = apiServiceProviderId;
            ProjectName = projectName;
            SubTitle = subTitle;
            Sort = sort;
            Grade = grade;
            Icon = icon;
            Description = description;
        }

    }
}
