using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users_usemobile_history")]
    public class UsersUseMobileHistory : EntityBase
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ChannelId { get; set; }

        [Required]
        public string ChannelName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; }
    }
}
