using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.Customers;

namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer, CreateCustomerDto, UpdateCustomerDto, CustomerResponseDto>
{
    Task<IEnumerable<CustomerResponseDto>> SearchByNameAsync(string searchTerm);
}
