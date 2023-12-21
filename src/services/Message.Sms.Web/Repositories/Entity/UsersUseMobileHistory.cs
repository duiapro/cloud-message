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
        [MaxLength(30)]
        public string ApiServiceProviderType { get; set; }

        [Required]
        public string ChannelName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; }

        public UsersUseMobileHistory(Guid userId, Guid channelId, string apiServiceProviderType, string channelName, string mobile)
        {
            UserId = userId;
            ChannelId = channelId;
            ApiServiceProviderType = apiServiceProviderType;
            ChannelName = channelName;
            Mobile = mobile;
        }

        public UsersUseMobileHistory()
        {
        }
    }
}
