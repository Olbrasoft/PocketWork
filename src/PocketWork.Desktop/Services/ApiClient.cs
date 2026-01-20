using System.Net.Http.Json;
using Olbrasoft.PocketWork.Desktop.Models;

namespace Olbrasoft.PocketWork.Desktop.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Customers
    public async Task<IEnumerable<CustomerModel>> GetCustomersAsync()
    {
        var response = await _httpClient.GetAsync("/api/customers");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>()
            ?? Enumerable.Empty<CustomerModel>();
    }

    public async Task<CustomerModel?> GetCustomerAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/customers/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CustomerModel>();
    }

    public async Task<CustomerModel> CreateCustomerAsync(CreateCustomerModel customer)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/customers", customer);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CustomerModel>()
            ?? throw new InvalidOperationException("Failed to deserialize created customer");
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/customers/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Orders
    public async Task<IEnumerable<OrderModel>> GetOrdersAsync()
    {
        var response = await _httpClient.GetAsync("/api/orders");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<OrderModel>>()
            ?? Enumerable.Empty<OrderModel>();
    }

    public async Task<OrderModel?> GetOrderAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/orders/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderModel>();
    }

    public async Task<OrderModel> CreateOrderAsync(CreateOrderModel order)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/orders", order);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderModel>()
            ?? throw new InvalidOperationException("Failed to deserialize created order");
    }

    public async Task DeleteOrderAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/orders/{id}");
        response.EnsureSuccessStatusCode();
    }
}
