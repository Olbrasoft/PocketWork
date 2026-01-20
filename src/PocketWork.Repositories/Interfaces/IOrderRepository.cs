using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.Orders;

namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order, CreateOrderDto, UpdateOrderDto, OrderResponseDto>
{
    Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
}
