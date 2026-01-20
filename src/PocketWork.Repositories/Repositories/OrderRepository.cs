using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.Orders;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Repositories.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PocketWorkDbContext _context;

    public OrderRepository(PocketWorkDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order == null ? null : MapToResponseDto(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto createDto)
    {
        var order = new Order
        {
            CustomerId = createDto.CustomerId,
            OrderType = createDto.OrderType,
            OrderDate = createDto.OrderDate,
            OrderTime = createDto.OrderTime,
            ReservedTime = createDto.ReservedTime
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();

        return MapToResponseDto(order);
    }

    public async Task UpdateAsync(int id, UpdateOrderDto updateDto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with id {id} not found");
        }

        order.OrderType = updateDto.OrderType;
        order.OrderDate = updateDto.OrderDate;
        order.OrderTime = updateDto.OrderTime;
        order.ReservedTime = updateDto.ReservedTime;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime from, DateTime to)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= from && o.OrderDate <= to)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer != null
                ? $"{order.Customer.Name} {order.Customer.Surname}"
                : null,
            OrderType = order.OrderType,
            OrderDate = order.OrderDate,
            OrderTime = order.OrderTime,
            ReservedTime = order.ReservedTime
        };
    }
}
