using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Olbrasoft.PocketWork.Api.Controllers;
using Olbrasoft.PocketWork.Repositories.DTOs.Customers;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Api.Tests;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<ILogger<CustomersController>> _mockLogger;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockLogger = new Mock<ILogger<CustomersController>>();
        _controller = new CustomersController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsOkWithCustomers()
    {
        // Arrange
        var customers = new List<CustomerResponseDto>
        {
            new() { Id = 1, Name = "John", Surname = "Doe", Email = "john@test.com", PhoneNumber = "111" },
            new() { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane@test.com", PhoneNumber = "222" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        // Act
        var result = await _controller.GetAllCustomers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerResponseDto>>(okResult.Value);
        Assert.Equal(2, returnedCustomers.Count());
    }

    [Fact]
    public async Task GetCustomer_ExistingId_ReturnsOkWithCustomer()
    {
        // Arrange
        var customer = new CustomerResponseDto { Id = 1, Name = "John", Surname = "Doe", Email = "john@test.com", PhoneNumber = "111" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

        // Act
        var result = await _controller.GetCustomer(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomer = Assert.IsType<CustomerResponseDto>(okResult.Value);
        Assert.Equal(1, returnedCustomer.Id);
    }

    [Fact]
    public async Task GetCustomer_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((CustomerResponseDto?)null);

        // Act
        var result = await _controller.GetCustomer(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCustomer_ValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateCustomerDto { Name = "New", Surname = "Customer", Email = "new@test.com", PhoneNumber = "333" };
        var createdCustomer = new CustomerResponseDto { Id = 1, Name = "New", Surname = "Customer", Email = "new@test.com", PhoneNumber = "333" };
        _mockRepository.Setup(r => r.CreateAsync(createDto)).ReturnsAsync(createdCustomer);

        // Act
        var result = await _controller.CreateCustomer(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(CustomersController.GetCustomer), createdResult.ActionName);
    }

    [Fact]
    public async Task DeleteCustomer_ExistingId_ReturnsNoContent()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCustomer(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
