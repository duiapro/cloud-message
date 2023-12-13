namespace Message.Sms.Web.OpenSDK.ApiService
{
    public abstract class SmsApiClientBase
    {
        public abstract string ServiceType { get; }

        private readonly ApiClientTokenManage _tokenManage;

        public SmsApiClientBase(ApiClientTokenManage tokenManage)
        {
            _tokenManage = tokenManage;
        }

        protected virtual string? GetToken(string? token = "")
        {
            if (!string.IsNullOrEmpty(token))
                return token;

            return _tokenManage.GetToken(ServiceType) ?? throw new Exception("There is an abnormality in the channel, please contact customer service");
        }
    }
}
