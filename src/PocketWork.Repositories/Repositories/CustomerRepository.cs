using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.Customers;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Repositories.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly PocketWorkDbContext _context;

    public CustomerRepository(PocketWorkDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);

        return customer == null ? null : MapToResponseDto(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        var customers = await _context.Customers
            .Include(c => c.Orders)
            .ToListAsync();

        return customers.Select(MapToResponseDto);
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto createDto)
    {
        var customer = new Customer
        {
            Name = createDto.Name,
            Surname = createDto.Surname,
            PhoneNumber = createDto.PhoneNumber,
            Email = createDto.Email,
            Address = createDto.Address
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return MapToResponseDto(customer);
    }

    public async Task UpdateAsync(int id, UpdateCustomerDto updateDto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with id {id} not found");
        }

        customer.Name = updateDto.Name;
        customer.Surname = updateDto.Surname;
        customer.PhoneNumber = updateDto.PhoneNumber;
        customer.Email = updateDto.Email;
        customer.Address = updateDto.Address;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CustomerResponseDto>> SearchByNameAsync(string searchTerm)
    {
        var customers = await _context.Customers
            .Include(c => c.Orders)
            .Where(c => c.Name.Contains(searchTerm) || c.Surname.Contains(searchTerm))
            .ToListAsync();

        return customers.Select(MapToResponseDto);
    }

    private static CustomerResponseDto MapToResponseDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Surname = customer.Surname,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address,
            OrdersCount = customer.Orders.Count
        };
    }
}
