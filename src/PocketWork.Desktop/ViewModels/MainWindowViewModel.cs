using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Olbrasoft.PocketWork.Desktop.Models;
using Olbrasoft.PocketWork.Desktop.Services;

namespace PocketWork.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _customers = [];

    [ObservableProperty]
    private ObservableCollection<OrderModel> _orders = [];

    [ObservableProperty]
    private CustomerModel? _selectedCustomer;

    [ObservableProperty]
    private OrderModel? _selectedOrder;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    // New customer form fields
    [ObservableProperty]
    private string _newCustomerName = string.Empty;

    [ObservableProperty]
    private string _newCustomerSurname = string.Empty;

    [ObservableProperty]
    private string _newCustomerEmail = string.Empty;

    [ObservableProperty]
    private string _newCustomerPhone = string.Empty;

    // New order form fields
    [ObservableProperty]
    private DateTime _newOrderDate = DateTime.Today;

    [ObservableProperty]
    private string _newOrderTime = "10:00";

    [ObservableProperty]
    private int _newOrderType = 1;

    public MainWindowViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        await Task.WhenAll(LoadCustomersAsync(), LoadOrdersAsync());
    }

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading customers...";
            var customers = await _apiClient.GetCustomersAsync();
            Customers = new ObservableCollection<CustomerModel>(customers);
            StatusMessage = $"Loaded {Customers.Count} customers";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading customers: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadOrdersAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading orders...";
            var orders = await _apiClient.GetOrdersAsync();
            Orders = new ObservableCollection<OrderModel>(orders);
            StatusMessage = $"Loaded {Orders.Count} orders";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading orders: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateCustomerAsync()
    {
        if (string.IsNullOrWhiteSpace(NewCustomerName) ||
            string.IsNullOrWhiteSpace(NewCustomerSurname) ||
            string.IsNullOrWhiteSpace(NewCustomerEmail))
        {
            StatusMessage = "Please fill in all required fields";
            return;
        }

        try
        {
            IsLoading = true;
            StatusMessage = "Creating customer...";

            var newCustomer = new CreateCustomerModel
            {
                Name = NewCustomerName,
                Surname = NewCustomerSurname,
                Email = NewCustomerEmail,
                PhoneNumber = NewCustomerPhone
            };

            await _apiClient.CreateCustomerAsync(newCustomer);

            // Clear form
            NewCustomerName = string.Empty;
            NewCustomerSurname = string.Empty;
            NewCustomerEmail = string.Empty;
            NewCustomerPhone = string.Empty;

            // Reload customers
            await LoadCustomersAsync();
            StatusMessage = "Customer created successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error creating customer: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer == null)
        {
            StatusMessage = "Please select a customer to delete";
            return;
        }

        try
        {
            IsLoading = true;
            StatusMessage = "Deleting customer...";
            await _apiClient.DeleteCustomerAsync(SelectedCustomer.Id);
            await LoadCustomersAsync();
            await LoadOrdersAsync();
            StatusMessage = "Customer deleted successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting customer: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateOrderAsync()
    {
        if (SelectedCustomer == null)
        {
            StatusMessage = "Please select a customer first";
            return;
        }

        if (!TimeSpan.TryParse(NewOrderTime, out var orderTime))
        {
            StatusMessage = "Invalid time format. Use HH:mm";
            return;
        }

        try
        {
            IsLoading = true;
            StatusMessage = "Creating order...";

            var newOrder = new CreateOrderModel
            {
                CustomerId = SelectedCustomer.Id,
                OrderType = NewOrderType,
                OrderDate = NewOrderDate,
                OrderTime = orderTime,
                ReservedTime = TimeSpan.FromHours(1)
            };

            await _apiClient.CreateOrderAsync(newOrder);

            // Reset form
            NewOrderDate = DateTime.Today;
            NewOrderTime = "10:00";
            NewOrderType = 1;

            // Reload orders
            await LoadOrdersAsync();
            await LoadCustomersAsync();
            StatusMessage = "Order created successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error creating order: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteOrderAsync()
    {
        if (SelectedOrder == null)
        {
            StatusMessage = "Please select an order to delete";
            return;
        }

        try
        {
            IsLoading = true;
            StatusMessage = "Deleting order...";
            await _apiClient.DeleteOrderAsync(SelectedOrder.Id);
            await LoadOrdersAsync();
            await LoadCustomersAsync();
            StatusMessage = "Order deleted successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting order: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAllAsync()
    {
        await LoadDataAsync();
    }
}
