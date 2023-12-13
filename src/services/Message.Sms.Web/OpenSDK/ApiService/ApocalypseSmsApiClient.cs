using Message.Sms.Web.OpenSDK.Models;
using Newtonsoft.Json;

namespace Message.Sms.Web.OpenSDK.ApiService
{
    public class ApocalypseSmsApiClient : SmsApiClientBase
    {
        private readonly HttpClient _httpClient;

        public override string ServiceType => GetServiceType;

        public static readonly string GetServiceType = nameof(ApocalypseSmsApiClient);

        public ApocalypseSmsApiClient(IHttpClientFactory httpClientFactory, ApiClientTokenManage tokenManage) : base(tokenManage)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(ApocalypseSmsApiClient));
        }

        public async Task<dynamic> LoginAsync(string username, string password)
        {
            var response = await _httpClient.GetAsync($"/api/login?username={username}&password={password}");
            return ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
        }

        public async Task<dynamic> GetWalletAsync(string token = "")
        {
            var response = await _httpClient.GetAsync($"/api/getWallet?token={this.GetToken(token)}");
            return ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
        }

        public async Task<dynamic> GetPhoneAsync(string channelId, string phoneNum, string operators, string scope, string token = "")
        {
            var response = await _httpClient.GetAsync($"/api/getPhone?token={this.GetToken(token)}&channelId={channelId}&phoneNum={phoneNum}&operators={operators}&scope={scope}");
            return ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
        }

        public async Task<dynamic> GetPhoneCodeAsync(string channelId, string phoneNum, string token = "")
        {
            var response = await _httpClient.GetAsync($"/api/getCode?token={this.GetToken(token)}&channelId={channelId}&phoneNum={phoneNum}");
            return ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
        }

        private static T ConvertResult<T>(string value)
        {
            var result = JsonConvert.DeserializeObject<ApocalypseResponseBase<T>>(value);
            if (result is not null && result.Success)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result?.Msg.ToString());
            }
        }
    }

}
