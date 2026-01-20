using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore;
using Olbrasoft.PocketWork.Repositories.DTOs.Customers;
using Olbrasoft.PocketWork.Repositories.Repositories;

namespace Olbrasoft.PocketWork.Repositories.Tests;

public class CustomerRepositoryTests
{
    private static PocketWorkDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PocketWorkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new PocketWorkDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsResponseDto()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);
        var createDto = new CreateCustomerDto
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123456789",
            Address = "123 Main St"
        };

        // Act
        var result = await repository.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("John", result.Name);
        Assert.Equal("Doe", result.Surname);
        Assert.Equal("John Doe", result.FullName);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCustomer()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);
        var created = await repository.CreateAsync(new CreateCustomerDto
        {
            Name = "Jane",
            Surname = "Smith",
            Email = "jane@example.com",
            PhoneNumber = "987654321"
        });

        // Act
        var result = await repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Jane", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_WithCustomers_ReturnsAll()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);
        await repository.CreateAsync(new CreateCustomerDto { Name = "Customer1", Surname = "One", Email = "c1@test.com", PhoneNumber = "111" });
        await repository.CreateAsync(new CreateCustomerDto { Name = "Customer2", Surname = "Two", Email = "c2@test.com", PhoneNumber = "222" });

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ExistingCustomer_UpdatesSuccessfully()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);
        var created = await repository.CreateAsync(new CreateCustomerDto
        {
            Name = "Original",
            Surname = "Name",
            Email = "original@test.com",
            PhoneNumber = "111"
        });

        // Act
        await repository.UpdateAsync(created.Id, new UpdateCustomerDto
        {
            Name = "Updated",
            Surname = "Name",
            Email = "updated@test.com",
            PhoneNumber = "222"
        });
        var result = await repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
        Assert.Equal("updated@test.com", result.Email);
    }

    [Fact]
    public async Task DeleteAsync_ExistingCustomer_RemovesFromDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new CustomerRepository(context);
        var created = await repository.CreateAsync(new CreateCustomerDto
        {
            Name = "ToDelete",
            Surname = "Customer",
            Email = "delete@test.com",
            PhoneNumber = "333"
        });

        // Act
        await repository.DeleteAsync(created.Id);
        var result = await repository.GetByIdAsync(created.Id);

        // Assert
        Assert.Null(result);
    }
}
