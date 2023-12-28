using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("api_serviceprovider")]
    public class ApiServiceProvider : EntityBase
    {
        public string Type { get; set; } = string.Empty;

        public string Account { get; set; } = string.Empty;

        public string PassWord { get; set; } = string.Empty;

        public string Authority { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public bool EnableTest { get; set; } = false;

        private List<Channel> _channels = new List<Channel>();

        public IReadOnlyCollection<Channel> Channels => _channels;

        public ApiServiceProvider()
        {
        }

        public ApiServiceProvider(string type, string account, string passWord, string authority, string remark,
            bool enableTest = false)
        {
            EnableTest = enableTest;
            Type = type;
            Account = account;
            PassWord = passWord;
            Authority = authority;
            Remark = remark;
        }
    }
}