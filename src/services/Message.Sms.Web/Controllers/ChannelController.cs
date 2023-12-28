using Message.Sms.Web.Infrastructure.Tools;
using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.Repositories.Entity;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Message.Sms.Web.Infrastructure;

namespace Message.Sms.Web.Controllers
{
    //[AuthFilter]
    public class ChannelController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ChannelController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string searchString, Guid? serviceProviderId)
        {
            System.Linq.Expressions.Expression<Func<Channel, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = channel => channel.Name.Contains(searchString);
            }
            if (serviceProviderId.HasValue)
            {
                filter = channel => channel.ApiServiceProviderId == serviceProviderId;
            }
            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.ServiceProviderId = serviceProviderId;
            ViewData["ChannelData"] = await _dbContext.Channels.Where(filter).ToListAsync();
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();

            return View();
        }

        public async Task<IActionResult> Message(Guid keyId)
        {
            var channel = await _dbContext.Channels.FindAsync(keyId);
            if (channel == null)
            {
                throw new Exception("KeyId does not exist.");
            }
            ViewData["ChannelData"] = channel;
            ViewData["Title"] = $"{channel.Name}-详情";
            return View();
        }

        public async Task<IActionResult> UpdatePartialView(Guid keyId)
        {
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return PartialView("./Component/_Update", await _dbContext.Channels.FindAsync(keyId));
        }

        [AuthFilter(IsAdmin = false)]
        public async Task<IActionResult> PhoneNumberLogs(int pageIndex = 1, int pageSize = 10, string searchString = "")
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;


            var fromQuery = (from codeLogs in _dbContext.UsersSmsCodeLogs
                             join channel in _dbContext.Channels on codeLogs.ChannelId equals channel.KeyId
                             select new UsersSmsCodeLogsViewModel
                             {
                                 ChannelId = codeLogs.ChannelId,
                                 UserId = codeLogs.UserId,
                                 ChannelCode = codeLogs.ChannelCode,
                                 ApiServiceProviderType = codeLogs.ApiServiceProviderType,
                                 Mobile = codeLogs.Mobile,
                                 Code = codeLogs.Code,
                                 Context = codeLogs.Context,
                                 Status = codeLogs.Status,
                                 CreateTime = codeLogs.CreateTime,
                                 Price = channel.Price,
                                 ChannelName = channel.Name,
                             }
                         );
            System.Linq.Expressions.Expression<Func<UsersSmsCodeLogsViewModel, bool>> filter = item => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                fromQuery.Where(item => item.Context.Contains(searchString) || item.Mobile.Contains(searchString));
            }
            if (!LoginUser.IsAdmin.Value)
            {
                filter = item => item.UserId == LoginUser.KeyId;
            }
            var totalCount = await fromQuery.CountAsync();
            var data = await fromQuery.OrderByDescending(user => user.CreateTime).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            ViewData["Data"] = data;
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

        [HttpPost]
        [AuthFilter]
        [ValidateAntiForgeryToken]
        public async Task Create(CreateChannelViewModel model)
        {
            if (!_dbContext.ApiServiceProviders.Any(provider => provider.KeyId == model.ApiServiceProviderId))
            {
                throw new Exception("ServiceProviders not exists.");
            }

            if (_dbContext.Channels.Any(channel => channel.ApiServiceProviderId == model.ApiServiceProviderId && channel.Code == model.Code))
            {
                throw new Exception("Code already exists.");
            }

            var icon = await UploadHelper.ImageAsync(model.Icon);
            await _dbContext.Channels.AddAsync(new(model.ApiServiceProviderId, model.Code, model.Name, icon,
                model.Price, model.CostPrice, model.Description));
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthFilter]
        public async Task DeleteAsync(Guid keyId)
        {
            var channel = await _dbContext.Channels.FindAsync(keyId);
            if (channel != null)
            {
                _dbContext.Channels.Remove(channel);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("KeyId does not exist.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthFilter]
        public async Task UpdateAsync(UpdateChannelViewModel model)
        {
            var channel = await _dbContext.Channels.FindAsync(model.KeyId);
            if (channel != null)
            {
                channel.Code = model.Code;
                channel.Name = model.Name;
                channel.Price = model.Price;
                channel.CostPrice = model.CostPrice;
                channel.IsActive = model.IsActive;
                channel.Description = model.Description;
                if (model.Icon != null)
                {
                    channel.Icon = await UploadHelper.ImageAsync(model.Icon) ?? string.Empty;
                }
                _dbContext.Update(channel);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("KeyId does not exist.");
            }
        }
    }
}
