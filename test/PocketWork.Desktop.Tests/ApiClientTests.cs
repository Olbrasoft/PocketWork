using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using Olbrasoft.PocketWork.Desktop.Services;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

namespace Olbrasoft.PocketWork.Desktop.Tests;

public class ApiClientTests
{
    private static HttpClient CreateMockHttpClient(HttpResponseMessage response)
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:7001")
        };
    }

    [Fact]
    public async Task GetCustomersAsync_SuccessResponse_ReturnsCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Id = 1, Name = "John", Surname = "Doe", Email = "john@test.com", PhoneNumber = "111" },
            new() { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane@test.com", PhoneNumber = "222" }
        };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(customers)
        };
        var httpClient = CreateMockHttpClient(response);
        var apiClient = new ApiClient(httpClient);

        // Act
        var result = await apiClient.GetCustomersAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCustomerAsync_NotFound_ReturnsNull()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var httpClient = CreateMockHttpClient(response);
        var apiClient = new ApiClient(httpClient);

        // Act
        var result = await apiClient.GetCustomerAsync(999);

        // Assert
        Assert.Null(result);
    }
}
