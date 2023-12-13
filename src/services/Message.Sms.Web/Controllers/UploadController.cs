using Microsoft.AspNetCore.Mvc;

namespace Message.Sms.Web.Controllers
{
    public class UploadController : Controller
    {
        public async Task<IActionResult> ImagesAsync()
        {
            return View();
        }
    }
}
