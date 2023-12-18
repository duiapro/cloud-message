using Message.Sms.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Areas.User.Controllers
{
    [Area("User")]
    public class AreasControllerBase : Controller
    {
        public AppUsers AppUsers => HttpContext.RequestServices.GetService<AppUsers>()!;

    }
}
