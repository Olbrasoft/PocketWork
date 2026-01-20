using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;

namespace Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

public sealed class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public JobType JobType { get; set; }
}
