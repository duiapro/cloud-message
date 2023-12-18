using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("channel")]
    public class Channel : EntityBase
    {
        [Required]
        public Guid ApiServiceProviderId { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Icon { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Precision(18, 2)]
        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ApiServiceProvider ApiServiceProvider { get; set; }

        public Channel()
        {
        }

        public Channel(Guid apiServiceProviderId, string code, string name, string icon, decimal price, decimal costPrice, string description, bool isActive = true)
        {
            Code = code;
            Name = name;
            Icon = icon ?? string.Empty;
            Price = price;
            CostPrice = costPrice;
            IsActive = isActive;
            Description = description ?? string.Empty;
            ApiServiceProviderId = apiServiceProviderId;
        }
    }
}
