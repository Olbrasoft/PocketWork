namespace Olbrasoft.PocketWork.Repositories.DTOs.Customers;

public record UpdateCustomerDto
{
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Address { get; init; }
}
