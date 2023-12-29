using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Linq;

namespace Message.Sms.Web.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IDistributedCache _distributedCache;

        public UsersController(AppDbContext dbContext, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _distributedCache = distributedCache;
        }

        [AuthFilter(IsAdmin = true)]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchString = "")
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            System.Linq.Expressions.Expression<Func<Users, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = channel =>
                    channel.UserName.Contains(searchString) || channel.UserMobile.Contains(searchString);
            }

            var users = _dbContext.Users.Where(filter);
            var totalCount = await users.CountAsync();
            var userData = await users.OrderByDescending(user => user.CreateTime).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            ViewData["Users"] = userData;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = (totalCount + pageSize - 1) / pageSize;
            ;
            ViewBag.Skip = (pageIndex - 1) * pageSize;
            ViewBag.SkipLast = ViewBag.Skip + pageSize;
            ViewBag.SearchString = searchString ?? string.Empty;
            return View();
        }

        [AuthFilter(IsAdmin = false)]
        public async Task<IActionResult> Details()
        {
            var users = await _dbContext.Users.Where(user => user.KeyId == LoginUser.KeyId).FirstOrDefaultAsync();
            var usersSmsCodeLogs = _dbContext.UsersSmsCodeLogs.Where(usersSmsCodeLogs => usersSmsCodeLogs.UserId == LoginUser.KeyId)
                .Select(usersSmsCodeLogs =>
                new { usersSmsCodeLogs.Status, usersSmsCodeLogs.Code, usersSmsCodeLogs.UserId });

            ViewBag.RunningTask = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 2);
            ViewBag.Failed = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 3 && logs.Code == string.Empty);
            ViewBag.Complete = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 3 && logs.Code != string.Empty);

            return View(users);
        }

        public async Task<IActionResult> ConsumeBill(int pageIndex = 1, int pageSize = 20, string startDate = "",
            string entDate = "", int? type = null)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            System.Linq.Expressions.Expression<Func<UsersBalanceBill, bool>> filter = item => true;
            if (type.HasValue)
            {
                filter = item => item.Type == type;
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                if (DateTime.TryParse(startDate, out var startDateTime))
                    filter = item => item.CreateTime >= startDateTime;
            }

            if (!string.IsNullOrEmpty(entDate))
            {
                if (DateTime.TryParse(entDate, out var endDateTime))
                    filter = item => item.CreateTime < endDateTime;
            }
            if (!LoginUser.IsAdmin.Value)
            {
                filter = item => item.UserKeyId == LoginUser.KeyId;
            }

            var usersBalanceBills = _dbContext.UsersBalanceBills.Where(filter);
            var totalCount = await usersBalanceBills.CountAsync();
            var data = await usersBalanceBills.OrderByDescending(user => user.CreateTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = (totalCount + pageSize - 1) / pageSize;
            ViewBag.Skip = (pageIndex - 1) * pageSize;
            ViewBag.SkipLast = ViewBag.Skip + pageSize;
            ViewBag.Type = type;
            ViewBag.StartDate = startDate;
            ViewBag.EntDate = entDate;

            return PartialView("./Component/_ConsumeBill", data);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task RechargeBalance(string cardPassword)
        {
            if (string.IsNullOrEmpty(cardPassword))
            {
                throw new Exception("cardPassword is not null.");
            }

            if (!Guid.TryParse(cardPassword, out var code))
            {
                throw new Exception("cardPassword error! ");
            }

            var rechargeCard = await _dbContext.RechargeCards.FirstOrDefaultAsync(card => card.Code == code);
            _ = rechargeCard ?? throw new Exception("cardPassword does not exist! ");

            var users = await _dbContext.Users.FirstOrDefaultAsync(users => users.KeyId == LoginUser.KeyId);
            _ = users ?? throw new Exception("this login user error !");

            users.RechargeBalance(rechargeCard.Amount, $"Card Password Recharge", $"use {cardPassword}");
            rechargeCard.VerifyAndUse();

            _dbContext.Update(users);
            _dbContext.Update(rechargeCard);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.AsNoTracking()
                    .FirstOrDefaultAsync(user => user.UserMobile == model.Mobile);
                if (user is not null && model.Password == user.PassWork)
                {
                    base.AppUsers.Set(new(user.KeyId, user.UserName, user.UserMobile, user.Balance, user.IsVip,
                        user.Discount, user.IsAdmin));
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
                    return Redirect("/users/Login");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.UserMobile), "The mobile phone number has been registered.");
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            base.AppUsers.Clear();
            return Redirect("/users/Login");
        }
    }
}