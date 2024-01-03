using Message.Sms.Web.Infrastructure;
using Message.Sms.Web.Infrastructure.Tools;
using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.OpenSDK.Models;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories;
using Message.Sms.Web.Repositories.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Message.Sms.Web.Controllers
{
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly SmsApiClientAdapter _smsApiClientAdapter;
        private readonly IDistributedCache _distributedCache;

        public ProjectController(AppDbContext dbContext, SmsApiClientAdapter smsApiClientAdapter,
            IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _smsApiClientAdapter = smsApiClientAdapter;
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index(string searchString, bool? status = true)
        {
            System.Linq.Expressions.Expression<Func<Project, bool>> filter = channel => true;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = project => project.ProjectName.Contains(searchString);
            }

            if (status.HasValue)
            {
                filter = project => project.Status == status;
            }

            ViewBag.SearchString = searchString ?? string.Empty;
            ViewBag.Status = status;
            ViewData["Data"] = await _dbContext.Projects.Where(filter).OrderByDescending(project => project.CreateTime)
                .ToListAsync();

            var usersSmsCodeLogs = _dbContext.UsersSmsCodeLogs.Where(usersSmsCodeLogs => usersSmsCodeLogs.UserId == LoginUser.KeyId)
                .Select(usersSmsCodeLogs =>
                new { usersSmsCodeLogs.Status, usersSmsCodeLogs.Code, usersSmsCodeLogs.UserId });

            ViewBag.RunningTask = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 2);
            ViewBag.Failed = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 3 && logs.Code == string.Empty);
            ViewBag.Complete = await usersSmsCodeLogs.CountAsync(logs => logs.Status == 3 && logs.Code != string.Empty);

            ViewData["User"] = await _dbContext.Users.Where(user => user.KeyId == LoginUser.KeyId).FirstOrDefaultAsync();

            return View();

        }

        [AuthFilter]
        public async Task<IActionResult> Create()
        {
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return View(new CreateProjectViewModel());
        }

        public async Task<IActionResult> Details(Guid keyId, string? searchString)
        {
            var projects = await _dbContext.Projects.FindAsync(keyId);
            _ = projects ?? throw new Exception("keyId not exists.");

            var modelCache = await _distributedCache.GetOrCreateAsync<List<ApocalypseGetChanneData>>(keyId.ToString(),
                async (cacheEnty) =>
                {
                    cacheEnty.SetAbsoluteExpiration(TimeSpan.FromHours(24));


                    var apocalypseProject =
                        await _smsApiClientAdapter.GetProjectAsync<ApocalypseGetProjectResponse>(
                            projects.ApiServiceProviderType, projects.ProjectName);

                    var model = new List<ApocalypseGetChanneData>();
                    foreach (var item in apocalypseProject.List) //.FindAll(pro => pro.name.Contains("10"))
                    {
                        var channels = await _smsApiClientAdapter.GetChannelAsync<ApocalypseGetChannelResponse>(
                            projects.ApiServiceProviderType,
                            new ApocalypseGetGetChannelsRequest(item.channelIdList));
                        if (channels is not null)
                        {
                            model.AddRange(channels.List);
                        }
                    }

                    return model;
                });
            if (!string.IsNullOrEmpty(searchString))
            {
                modelCache = modelCache?.FindAll(item => !string.IsNullOrEmpty(item.province) && item.province.Contains(searchString));
            }
            ViewBag.Icon = projects.Icon;
            ViewBag.KeyId = keyId;
            ViewBag.SearchString = searchString;
            return View(modelCache);
        }

        public async Task<IActionResult> UpdatePartialView(Guid keyId)
        {
            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return PartialView("./Component/_Update", await _dbContext.Channels.FindAsync(keyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthFilter]
        public async Task<IActionResult> Create(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var apiServiceProviders =
                    await _dbContext.ApiServiceProviders.FirstOrDefaultAsync(provider =>
                        provider.KeyId == model.ApiServiceProviderId);
                if (apiServiceProviders is null)
                {
                    ModelState.AddModelError(nameof(model.ApiServiceProviderId), "ApiServiceProviderId not exist.");
                }

                if (_dbContext.Projects.Any(provider => provider.ProjectName == model.ProjectName))
                {
                    ModelState.AddModelError(nameof(model.ProjectName), "projectName exist.");
                }

                if (model.Icon is null)
                {
                    ModelState.AddModelError(nameof(model.Icon), "Icon is not.");
                }

                if (ModelState.ErrorCount == 0)
                {
                    var icon = await UploadHelper.ImageAsync(model.Icon);
                    await _dbContext.Projects.AddAsync(new(model.ApiServiceProviderId, apiServiceProviders.Type,
                        model.ProjectName, model.SubTitle, model.Sort,
                        model.Grade, icon, model.Description));
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ApiServiceProviderId"] = await _dbContext.ApiServiceProviders.ToListAsync();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthFilter]
        public async Task DeleteAsync(Guid keyId)
        {
            var data = await _dbContext.Projects.FindAsync(keyId);
            if (data != null)
            {
                _dbContext.Remove(data);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("KeyId does not exist.");
            }
        }

        public async Task<IActionResult> CreateChannel(Guid keyId, int channelId)
        {
            var modelCache = await _distributedCache.GetAsync<List<ApocalypseGetChanneData>>(keyId.ToString());

            if (modelCache == null || !modelCache.Any())
            {
                throw new Exception("keyId not exists.");
            }

            var projects = await _dbContext.Projects.FindAsync(keyId);
            _ = projects ?? throw new Exception("projects not exists.");
            var channelModel = modelCache.FirstOrDefault(channel => channel.id == channelId);
            _ = channelModel ?? throw new Exception("channelId not exists.");

            var channelIdModel =
                await _smsApiClientAdapter.GetChannelIdAsync(projects.ApiServiceProviderType, channelModel);

            var thisSysChannel = await _dbContext.Channels
                .Select(channel => new { channel.KeyId, channel.ApiServiceProviderId, channel.Code }).FirstOrDefaultAsync(channel =>
                    channel.ApiServiceProviderId == projects.ApiServiceProviderId &&
                    channel.Code == channelIdModel.ChannelId);
            var thisSysChannelId = thisSysChannel?.KeyId ?? Guid.Empty;
            if (thisSysChannel is null)
            {
                var userMoney = Convert.ToDecimal(channelModel.userMoney);
                var channelsEntity = await _dbContext.Channels.AddAsync(new(projects.ApiServiceProviderId,
                    channelIdModel.ChannelId, projects.ProjectName, projects.Icon,
                    userMoney + 1, userMoney,
                    $"Province：{channelModel.province}"));
                await _dbContext.SaveChangesAsync();
                thisSysChannelId = channelsEntity.Entity.KeyId;
            }

            return Redirect($"/channel/message/{thisSysChannelId}");
        }
    }
}