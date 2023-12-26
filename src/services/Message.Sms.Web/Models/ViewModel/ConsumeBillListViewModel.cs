namespace Message.Sms.Web.Models.ViewModel;

public class ConsumeBillListViewModel
{
    public Guid KeyId { get; set; }
    
    public Guid UserKeyId { get; set; }

    public string Title { get; set; } = string.Empty;

    //1.top up 2.deduct
    public int Type { get; set; }

    public decimal Amount { get; set; }

    public decimal BeforeBalance { get; set; }

    public decimal AfterBalance { get; set; }

    public string Remake { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}