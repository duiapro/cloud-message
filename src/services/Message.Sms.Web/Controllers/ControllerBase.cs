using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Controllers
{
    public class ControllerBase : Controller
    {
        public AppUsers AppUsers => HttpContext.RequestServices.GetService<AppUsers>()!;
    }
}
