using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("users_usemobile_codelogs")]
    public class UsersSmsCodeLogs : EntityBase
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ChannelId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ApiServiceProviderType { get; set; }

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(15)]
        public string Code { get; set; }

        [Required]
        public string Context { get; set; }

        //1.start 2.runing 3.end
        public int Status { get; set; }

        public virtual Users Users { get; set; }

        public UsersSmsCodeLogs(Guid userId, Guid channelId, string apiServiceProviderType, string mobile)
        {
            UserId = userId;
            ChannelId = channelId;
            ApiServiceProviderType = apiServiceProviderType;
            Mobile = mobile;
            Code = string.Empty;
            Context = string.Empty;
            Status = 1;
        }

        public void StartRuning()
        {
            Status = 2;
        }

        public void SetCode(string? code, string? context)
        {
            Code = code ?? string.Empty;
            Context = context ?? string.Empty;
            Status = 3;
        }
    }
}
