namespace Message.Sms.Web.OpenSDK
{
    public class ApiClientTokenManage
    {
        private readonly Dictionary<string, string> _token = new();

        public string? GetToken(string type)
        {
            if (_token.ContainsKey(type))
            {
                return _token[type];
            }
            else
            {
                return null;
            }
        }

        public void SetToken(string type, string token)
        {
            if (_token.ContainsKey(type))
            {
                _token[type] = token;
            }
            else
            {
                _token.Add(type, token);
            }
        }
    }
}
