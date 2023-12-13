using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Linq;

namespace Message.Sms.Web.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchString = "")
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            System.Linq.Expressions.Expression<Func<Users, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = channel => channel.UserName.Contains(searchString) || channel.UserMobile.Contains(searchString);
            }
            var users = _dbContext.Users.Where(filter);
            var totalCount = await users.CountAsync();
            var userData = await users.OrderByDescending(user => user.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewData["Users"] = userData;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = (totalCount + pageSize - 1) / pageSize; ;
            ViewBag.Skip = (pageIndex - 1) * pageSize;
            ViewBag.SkipLast = ViewBag.Skip + pageSize;
            ViewBag.SearchString = searchString ?? string.Empty;
            return View();
        }

        public IActionResult Details()
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
