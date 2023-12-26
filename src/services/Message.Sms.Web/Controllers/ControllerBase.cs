using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Controllers
{
    public class ControllerBase : Controller
    {
        public AppUsers AppUsers => HttpContext.RequestServices.GetService<AppUsers>()!;

        private AppUsers _loginUser;

        public AppUsers LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    _loginUser = AppUsers;
                }
                return _loginUser;
            }
        }
    }
}
