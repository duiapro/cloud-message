namespace Message.Sms.Web.OpenSDK.Models.Request
{
    public  class ApocalypseLoginRequest :RequestBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public ApocalypseLoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
