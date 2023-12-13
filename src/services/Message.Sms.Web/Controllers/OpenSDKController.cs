using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.Repositories.Entity;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Message.Sms.Web.OpenSDK.ApiService;

namespace Message.Sms.Web.Controllers
{
    public class OpenSDKController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ApocalypseSmsApiClient _smsApiClient;
        private readonly ApiClientTokenManage _apiClientTokenManage;

        public OpenSDKController(AppDbContext dbContext, ApocalypseSmsApiClient smsApiClient,
            ApiClientTokenManage apiClientTokenManage)
        {
            _dbContext = dbContext;
            _smsApiClient = smsApiClient;
            _apiClientTokenManage = apiClientTokenManage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task SavaApocalypseSmsAccount(CreateOpenSDKAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                async Task<string> LoginAsync(string username, string password)
                {
                    var result = await _smsApiClient.LoginAsync(model.Account, model.PassWord);
                    return result.token;
                }

                var apiAuthoritys = await _dbContext.ApiAuthoritys.FirstOrDefaultAsync(item => item.Type == nameof(ApocalypseSmsApiClient));
                if (apiAuthoritys is null)
                {
                    apiAuthoritys = new ApiAuthority()
                    {
                        Type = nameof(ApocalypseSmsApiClient),
                        Account = model.Account,
                        PassWord = model.PassWord,
                        Authority = await LoginAsync(model.Account, model.PassWord)
                    };

                    _dbContext.Add(apiAuthoritys);
                }
                else
                {
                    if (apiAuthoritys.Account != model.Account && apiAuthoritys.PassWord != model.PassWord && string.IsNullOrEmpty(apiAuthoritys.Authority))
                    {
                        apiAuthoritys.Authority = await LoginAsync(model.Account, model.PassWord);
                    }
                }

                //设置缓存
                _apiClientTokenManage.SetToken(nameof(ApocalypseSmsApiClient), apiAuthoritys.Authority);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("参数错误");
            }
        }
    }
}
