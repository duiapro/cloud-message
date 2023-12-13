namespace Message.Sms.Web.OpenSDK.Models
{
    public class ApocalypseResponseBase<T>
    {
        public T Data { get; set; }

        public int Status { get; set; }

        public bool Success { get; set; }

        public string Msg { get; set; }

        public object t { get; set; }
    }
}
