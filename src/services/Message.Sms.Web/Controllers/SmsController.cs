using Message.Sms.Web.OpenSDK;
using Message.Sms.Web.OpenSDK.ApiService;
using Message.Sms.Web.OpenSDK.Models.Request;
using Message.Sms.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Message.Sms.Web.Controllers
{
    public class SmsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly SmsApiClientAdapter _smsApiClientAdapter;

        public SmsController(AppDbContext dbContext, SmsApiClientAdapter smsApiClientAdapter)
        {
            _dbContext = dbContext;
            _smsApiClientAdapter = smsApiClientAdapter;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPhoneNumber(Guid channelKeyId)
        {
            var channel = await _dbContext.Channels.Include(channel => channel.ApiServiceProvider)
                .FirstOrDefaultAsync(channel => channel.KeyId == channelKeyId);

            _ = channel ?? throw new Exception("channelKeyId not querying valid");

            var client = _smsApiClientAdapter.Get(channel.ApiServiceProvider.Type);
            var phone = await client.GetPhoneAsync(channel.ApiServiceProvider.Type switch
            {
                nameof(ApocalypseSmsApiClient) => new ApocalypseGetPhoneRequest(channelId: channel.Code),
                //todo:..
                _ => throw new Exception("channelKeyId not querying valid")
            });

            if (false)//todo:login user
            {
                await _dbContext.UsersUseMobileHistorys.AddAsync(new(Guid.Empty, channelKeyId, channel.Name, phone.Mobile));
                await _dbContext.SaveChangesAsync();
            }

            return Content(phone.Mobile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetCode(Guid phoneNumberKeyId)
        {
            return Content("【新电途】您的验证码是160832。如非本人操作，请忽略本短信");
        }
    }
}