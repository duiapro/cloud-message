using Message.Sms.Web.OpenSDK.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

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
            _httpClient.DefaultRequestHeaders.Add("x-token", this.GetToken());
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

        public async Task<T> GetProjectAsync<T>(string projectName)
        {
            var url = $"/api/message/RegionCollect/searchProject";
            var result = await this.GetWebClient().InvokeAsync<object, ApocalypseResponseBase<T>>(HttpMethod.Post, url,
                new { search = projectName });
            return result.Data;
        }

        public async Task<T> GetChannelAsync<T>(RequestBase? request = null)
        {
            var url = $"/api/message/RegionCollect/search";
            var result = await this.GetWebClient(request?.ApiKey).InvokeAsync<object, ApocalypseResponseBase<T>>(HttpMethod.Post, url, request);
            return result.Data;
        }

        public async Task<ApocalypseGetChannelIdResposne> GetChannelIdAsync(RequestBase? request = null)
        {
            var url = $"/api/message/RegionCollect/add";
            var result = await this.GetWebClient(request?.ApiKey).InvokeAsync<object, ApocalypseResponseBase<ApocalypseGetChannelIdResposne>>(HttpMethod.Post, url, request);
            return result.Data;
        }

        private HttpClient GetWebClient(string? apiKey = null)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-token", this.GetToken(apiKey));
            httpClient.BaseAddress = new Uri("https://web.tqsms.xyz");
            return httpClient;
        }
    }
}