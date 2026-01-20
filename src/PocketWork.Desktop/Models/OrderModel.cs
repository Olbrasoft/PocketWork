namespace Olbrasoft.PocketWork.Desktop.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int OrderType { get; set; }
    public DateTime OrderDate { get; set; }
    public TimeSpan OrderTime { get; set; }
    public TimeSpan ReservedTime { get; set; }

    public string OrderTypeName => OrderType switch
    {
        1 => "Standard",
        2 => "Express",
        3 => "Subscription",
        _ => "Unknown"
    };

    public string FormattedDate => OrderDate.ToString("yyyy-MM-dd");
    public string FormattedTime => OrderTime.ToString(@"hh\:mm");
}

public class CreateOrderModel
{
    public int CustomerId { get; set; }
    public int OrderType { get; set; }
    public DateTime OrderDate { get; set; }
    public TimeSpan OrderTime { get; set; }
    public TimeSpan ReservedTime { get; set; } = TimeSpan.FromHours(1);
}
