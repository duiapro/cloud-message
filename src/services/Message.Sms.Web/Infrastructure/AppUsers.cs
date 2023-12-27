using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Message.Sms.Web.Infrastructure
{
    public class AppUsers
    {
        public const string SESSION_KEY = "Login.User";

        private readonly HttpContext? _httpContext;

        public AppUsers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        private ISession? Session => _httpContext?.Session;

        public Guid? KeyId => User?.KeyId;

        public string? UserName => User?.UserName;

        public string? UserMobile => User?.UserMobile;

        public decimal? Balance => User?.Balance;

        public bool? IsVip => User?.IsVip;

        public decimal? Discount => User?.Discount;

        public bool? IsAdmin => User?.IsAdmin;

        public bool IsLogin => User is not null;

        public void Set(LoginUserModel loginUser)
        {
            Session?.Set(SESSION_KEY, loginUser);
        }

        public void Clear()
        {
            Session?.Remove(SESSION_KEY);
        }

        public LoginUserModel? User => Session?.Get<LoginUserModel>(SESSION_KEY);
    }

    public class LoginUserModel
    {
        public LoginUserModel()
        {
        }

        public LoginUserModel(Guid keyId, string userName, string userMobile, decimal balance, bool isVip, decimal discount, bool isAdmin)
        {
            KeyId = keyId;
            UserName = userName;
            UserMobile = userMobile;
            Balance = balance;
            IsVip = isVip;
            Discount = discount;
            IsAdmin = isAdmin;
        }

        #region  Property

        public Guid KeyId { get; set; }

        public string UserName { get; set; }

        public string UserMobile { get; set; }

        public decimal Balance { get; set; }

        public bool IsVip { get; set; }

        public decimal Discount { get; set; } = 1;

        public bool IsAdmin { get; set; }

        #endregion
    }
}
