using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;
using Olbrasoft.PocketWork.Repositories.DTOs.Orders;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Mvc.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ILogger<OrdersController> logger)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetAllAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCustomersDropdown();
        PopulateOrderTypesDropdown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCustomersDropdown();
            PopulateOrderTypesDropdown();
            return View(dto);
        }

        await _orderRepository.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCustomersDropdown()
    {
        var customers = await _customerRepository.GetAllAsync();
        ViewBag.Customers = new SelectList(customers, "Id", "FullName");
    }

    private void PopulateOrderTypesDropdown()
    {
        var orderTypes = Enum.GetValues<OrderType>()
            .Where(t => t != OrderType.None)
            .Select(t => new { Value = (int)t, Text = t.ToString() });
        ViewBag.OrderTypes = new SelectList(orderTypes, "Value", "Text");
    }
}
