using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Models.ViewModel
{
    public class CreateChannelViewModel
    {
        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public IFormFile Icon { get; set; }

        public decimal Price { get; set; }

        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
