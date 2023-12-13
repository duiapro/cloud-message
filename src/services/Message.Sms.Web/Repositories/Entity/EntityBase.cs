using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Repositories.Entity
{
    public class EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public Guid KeyId { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
