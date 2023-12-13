using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Sms.Web.Repositories.Entity
{
    [Table("apiAuthority")]
    public class ApiAuthority : EntityBase
    {
        public string Type { get; set; } = string.Empty;

        public string Account { get; set; } = string.Empty;

        public string PassWord { get; set; } = string.Empty;

        public string Authority { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public ApiAuthority()
        {
        }

        public ApiAuthority(string type, string account, string passWord, string authority, string remark)
        {
            Type = type;
            Account = account;
            PassWord = passWord;
            Authority = authority;
            Remark = remark;
        }
    }
}
