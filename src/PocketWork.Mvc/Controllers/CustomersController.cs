using Microsoft.AspNetCore.Mvc;
using Olbrasoft.PocketWork.Repositories.DTOs.Customers;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Mvc.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await _customerRepository.GetAllAsync();
        return View(customers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _customerRepository.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        var dto = new UpdateCustomerDto
        {
            Name = customer.Name,
            Surname = customer.Surname,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        };
        ViewBag.CustomerId = id;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CustomerId = id;
            return View(dto);
        }

        try
        {
            await _customerRepository.UpdateAsync(id, dto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _customerRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
