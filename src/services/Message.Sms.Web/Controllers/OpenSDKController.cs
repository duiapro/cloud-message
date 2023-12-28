using Message.Sms.Web.Models.ViewModel;
using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.Repositories.Entity;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Message.Sms.Web.OpenSDK.ApiService;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Infrastructure;

namespace Message.Sms.Web.Controllers
{
    [AuthFilter]
    public class OpenSDKController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ApiClientTokenManage _apiClientTokenManage;
        private readonly SmsApiClientAdapter _smsApiClientAdapter;

        public OpenSDKController(AppDbContext dbContext,
            ApiClientTokenManage apiClientTokenManage, SmsApiClientAdapter smsApiClientAdapter)
        {
            _dbContext = dbContext;
            _apiClientTokenManage = apiClientTokenManage;
            _smsApiClientAdapter = smsApiClientAdapter;
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
                    var result =
                        await _smsApiClientAdapter.GetApocalypseSmsApiClient.LoginAsync(
                            new ApocalypseLoginRequest(username, password));
                    return result.ApiKey;
                }

                var apiAuthoritys =
                    await _dbContext.ApiServiceProviders.FirstOrDefaultAsync(item =>
                        item.Type == ApocalypseSmsApiClient.GetServiceType);
                if (apiAuthoritys is null)
                {
                    apiAuthoritys = new ApiServiceProvider()
                    {
                        Type = ApocalypseSmsApiClient.GetServiceType,
                        Account = model.Account,
                        PassWord = model.PassWord,
                        Remark = model.Remark,
                        Authority = await LoginAsync(model.Account, model.PassWord),
                        EnableTest = model.EnableTest
                    };

                    _dbContext.Add(apiAuthoritys);
                }
                else
                {
                    apiAuthoritys.EnableTest = model.EnableTest;
                    apiAuthoritys.Remark = model.Remark;
                    if (apiAuthoritys.Account != model.Account || apiAuthoritys.PassWord != model.PassWord ||
                        string.IsNullOrEmpty(apiAuthoritys.Authority))
                    {
                        apiAuthoritys.Authority = await LoginAsync(model.Account, model.PassWord);
                    }

                    _dbContext.Update(apiAuthoritys);
                }

                //设置缓存
                _apiClientTokenManage.SetToken(ApocalypseSmsApiClient.GetServiceType, new(apiAuthoritys.Authority, apiAuthoritys.EnableTest));

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("参数错误");
            }
        }
    }
}