namespace Message.Sms.Web.OpenSDK
{
    public class ApiClientTokenManage
    {
        private readonly Dictionary<string, ApiClientTokenData> _token = new();

        public string? GetToken(string type)
        {
            if (_token.ContainsKey(type))
            {
                return _token[type].Token;
            }
            else
            {
                return null;
            }
        }

        public bool IsTest(string type)
        {
            if (_token.ContainsKey(type))
            {
                return _token[type].IsTest;
            }
            else
            {
                return false;
            }
        }

        public void SetToken(string type, ApiClientTokenData data)
        {
            if (_token.ContainsKey(type))
            {
                _token[type] = data;
            }
            else
            {
                _token.Add(type, data);
            }
        }
    }

    public class ApiClientTokenData
    {
        public ApiClientTokenData(string token, bool isTest = false)
        {
            Token = token;
            IsTest = isTest;
        }

        public string Token { get; set; }

        public bool IsTest { get; set; }
    }
}
