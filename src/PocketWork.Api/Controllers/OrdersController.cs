using Microsoft.AspNetCore.Mvc;
using Olbrasoft.PocketWork.Repositories.DTOs.Orders;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto createDto)
    {
        try
        {
            var created = await _orderRepository.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto updateDto)
    {
        try
        {
            await _orderRepository.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByCustomer(int customerId)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        return Ok(orders);
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(from, to);
        return Ok(orders);
    }
}
