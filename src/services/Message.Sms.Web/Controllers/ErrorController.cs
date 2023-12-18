using Message.Sms.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Controllers
{
    [AuthFilter(false)]
    public class ErrorController : Controller
    {
        public IActionResult Index(string? message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
