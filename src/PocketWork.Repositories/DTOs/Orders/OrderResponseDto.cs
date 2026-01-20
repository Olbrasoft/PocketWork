using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.Repositories.DTOs.Orders;

public record OrderResponseDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public TimeSpan OrderTime { get; init; }
    public TimeSpan ReservedTime { get; init; }
}
