using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Areas.User.Controllers
{
    public class DefaultController : AreasControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DefaultController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserMobile == model.Mobile);
                if (user is not null && model.Password == user.PassWork)
                {
                    return Redirect("/Home/Index");
                }
                else if (user is null)
                {
                    ModelState.AddModelError(nameof(model.Mobile), "User does not exist, please register.");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "The password is incorrect.");
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.AnyAsync(user => user.UserMobile == model.UserMobile);
                if (!user)
                {
                    _dbContext.Users.Add(new(model.UserName, model.UserMobile, model.PassWork));
                    await _dbContext.SaveChangesAsync();
                    return RedirectToRoute(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError(nameof(model.UserMobile), "The mobile phone number has been registered.");
                }
            }

            return View(model);
        }
    }
}
