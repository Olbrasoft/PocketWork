using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.Repositories.DTOs.Orders;

public record UpdateOrderDto
{
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public TimeSpan OrderTime { get; init; }
    public TimeSpan ReservedTime { get; init; }
}
