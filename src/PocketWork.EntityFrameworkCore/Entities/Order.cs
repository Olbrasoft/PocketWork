using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

public sealed class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public OrderType OrderType { get; set; }
    public DateTime OrderDate { get; set; }
    public TimeSpan OrderTime { get; set; }
    public TimeSpan ReservedTime { get; set; }
}
