using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Message.Sms.Web.Controllers
{
    [AuthFilter]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!LoginUser.IsAdmin.Value)
                return RedirectToAction("index", "project");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
