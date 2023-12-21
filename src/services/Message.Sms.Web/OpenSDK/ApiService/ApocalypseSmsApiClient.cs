using Message.Sms.Web.OpenSDK.Models;
using Newtonsoft.Json;

namespace Message.Sms.Web.OpenSDK.ApiService
{
    public class ApocalypseSmsApiClient : SmsApiClientBase, ISmsApiClient
    {
        private readonly HttpClient _httpClient;

        public override string ServiceType => GetServiceType;

        public static readonly string GetServiceType = nameof(ApocalypseSmsApiClient);

        public ApocalypseSmsApiClient(IHttpClientFactory httpClientFactory, ApiClientTokenManage tokenManage) : base(
            tokenManage)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(ApocalypseSmsApiClient));
        }

        public async Task<LoginResponse> LoginAsync(RequestBase? request = null)
        {
            var parameter = request?.ToParameter();
            var response = await _httpClient.GetAsync($"/api/login?{parameter}");
            var apiResult = ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
            return new(apiResult.token);
        }

        public async Task<WalletResponse> GetWalletAsync(RequestBase? request = null)
        {
            var response = await _httpClient.GetAsync($"/api/getWallet?token={this.GetToken(request?.ApiKey)}");
            var result = ConvertResult<WalletResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        //(string channelId, string phoneNum, string operators, string scope,string token = "")
        public async Task<PhoneResponse> GetPhoneAsync(RequestBase? request = null)
        {
            var parameter = (request as object)?.ToParameter();
            var response = await _httpClient.GetAsync(
                $"/api/getPhone?token={this.GetToken(request?.ApiKey)}&{parameter}");
            var result = ConvertResult<PhoneResponse>(await response.Content.ReadAsStringAsync());
            return result;
        }

        //(string channelId, string phoneNum, string token = "")
        public async Task<PhoneCodeResponse> GetPhoneCodeAsync(RequestBase? request = null)
        {
            var parameter = request?.ToParameter();
            var response = await _httpClient.GetAsync(
                $"/api/getCode?token={this.GetToken(request?.ApiKey)}&{parameter}");
            var result = ConvertResult<dynamic>(await response.Content.ReadAsStringAsync());
            return new(result.code, result.modle);
        }

        private static T ConvertResult<T>(string value)
        {
            var result = JsonConvert.DeserializeObject<ApocalypseResponseBase<T>>(value);
            if (result is not null && result.Success)
            {
                return result.Data;
            }

            throw new Exception(result?.Msg.ToString());
        }
    }
}