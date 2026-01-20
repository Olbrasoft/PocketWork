namespace Olbrasoft.PocketWork.Repositories.DTOs.Customers;

public record CustomerResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string FullName => $"{Name} {Surname}";
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Address { get; init; }
    public int OrdersCount { get; init; }
}
