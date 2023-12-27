using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Infrastructure.Tools;
using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Controllers
{
    [AuthFilter]
    public class RechargeCardController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public RechargeCardController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string startDate = "",
            string entDate = "", string searchString = "", bool? isActive = null)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            System.Linq.Expressions.Expression<Func<RechargeCard, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(startDate))
            {
                if (DateTime.TryParse(startDate, out var startDateTime))
                    filter = card => card.StartTime >= startDateTime;
            }
            if (!string.IsNullOrEmpty(entDate))
            {
                if (DateTime.TryParse(entDate, out var endDateTime))
                    filter = card => card.EndTime < endDateTime;
            }
            if (isActive.HasValue)
            {
                filter = card => card.IsActive == isActive;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                //filter = channel => channel.UserName.Contains(searchString) || channel.UserMobile.Contains(searchString);
            }

            var rechargeCard = _dbContext.RechargeCards.Where(filter);
            var totalCount = await rechargeCard.CountAsync();
            var rechargeCardData = await rechargeCard.OrderByDescending(user => user.CreateTime)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewData["RechargeCard"] = rechargeCardData;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = (totalCount + pageSize - 1) / pageSize;
            ViewBag.Skip = (pageIndex - 1) * pageSize;
            ViewBag.SkipLast = ViewBag.Skip + pageSize;
            ViewBag.SearchString = searchString;
            ViewBag.StartDate = startDate;
            ViewBag.EntDate = entDate;
            ViewBag.IsActive = isActive;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create(CreateRechargeCardViewModel model)
        {
            if (model.Amount <= 0) throw new Exception("Amount error");
            List<RechargeCard> rechargeCards = new();
            for (var i = 0; i < model.Number; i++)
            {
                rechargeCards.Add(new(Guid.NewGuid(), model.Amount, model.StartTime, model.EndTime, model.Remark));
            }
            await _dbContext.RechargeCards.AddRangeAsync(rechargeCards);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task DeleteAsync(Guid keyId)
        {
            var rechargeCard = await _dbContext.RechargeCards.FindAsync(keyId);
            if (rechargeCard != null)
            {
                _dbContext.Remove(rechargeCard);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("KeyId does not exist.");
            }
        }
    }
}