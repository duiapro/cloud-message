namespace Message.Sms.Web.OpenSDK.Models;

public class WalletResponse
{
    public WalletResponse()
    {
    }

    public WalletResponse(decimal balances)
    {
        Balances = balances;
    }

    public decimal Balances { get; set; }
}