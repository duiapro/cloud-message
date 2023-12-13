using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Controllers
{
    public class SmsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPhoneNumber(Guid channelKeyId)
        {
            return Content("13800138000");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetCode(Guid phoneNumberKeyId)
        {
            return Content("【新电途】您的验证码是160832。如非本人操作，请忽略本短信");
        }
    }
}
