using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Areas.User.Controllers
{
    public class HomeController : AreasControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
