using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.EntityFrameworkCore.Tests;

public class PocketWorkDbContextTests
{
    private static PocketWorkDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PocketWorkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new PocketWorkDbContext(options);
    }

    [Fact]
    public async Task AddUser_ValidUser_SavesSuccessfully()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var user = new User
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123456789",
            JobType = JobType.Worker
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync();
        Assert.NotNull(savedUser);
        Assert.Equal("John", savedUser.Name);
        Assert.Equal("Doe", savedUser.Surname);
        Assert.Equal(JobType.Worker, savedUser.JobType);
    }

    [Fact]
    public async Task AddCustomer_ValidCustomer_SavesSuccessfully()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var customer = new Customer
        {
            Name = "Jane",
            Surname = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "987654321",
            Address = "123 Main St"
        };

        // Act
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        // Assert
        var savedCustomer = await context.Customers.FirstOrDefaultAsync();
        Assert.NotNull(savedCustomer);
        Assert.Equal("Jane", savedCustomer.Name);
        Assert.Equal("123 Main St", savedCustomer.Address);
    }

    [Fact]
    public async Task AddOrder_WithCustomer_SavesRelationship()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var customer = new Customer
        {
            Name = "Test",
            Surname = "Customer",
            Email = "test@example.com",
            PhoneNumber = "111222333"
        };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var order = new Order
        {
            CustomerId = customer.Id,
            OrderType = OrderType.Standard,
            OrderDate = DateTime.Today,
            OrderTime = TimeSpan.FromHours(10),
            ReservedTime = TimeSpan.FromHours(1)
        };

        // Act
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
        Assert.NotNull(savedOrder);
        Assert.NotNull(savedOrder.Customer);
        Assert.Equal("Test", savedOrder.Customer.Name);
    }
}
