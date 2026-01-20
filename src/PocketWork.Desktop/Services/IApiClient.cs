using Olbrasoft.PocketWork.Desktop.Models;

namespace Olbrasoft.PocketWork.Desktop.Services;

public interface IApiClient
{
    // Customers
    Task<IEnumerable<CustomerModel>> GetCustomersAsync();
    Task<CustomerModel?> GetCustomerAsync(int id);
    Task<CustomerModel> CreateCustomerAsync(CreateCustomerModel customer);
    Task DeleteCustomerAsync(int id);

    // Orders
    Task<IEnumerable<OrderModel>> GetOrdersAsync();
    Task<OrderModel?> GetOrderAsync(int id);
    Task<OrderModel> CreateOrderAsync(CreateOrderModel order);
    Task DeleteOrderAsync(int id);
}
