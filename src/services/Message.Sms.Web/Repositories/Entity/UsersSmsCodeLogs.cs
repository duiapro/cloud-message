﻿using System.ComponentModel.DataAnnotations;
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
        [MaxLength(15)]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(15)]
        public string Code { get; set; }

        [Required]
        public string Context { get; set; }

        public virtual Users Users { get; set; }
    }
}
