using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.Repositories.DTOs.Users;

public record UserResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string FullName => $"{Name} {Surname}";
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public JobType JobType { get; init; }
}
