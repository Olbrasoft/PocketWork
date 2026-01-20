namespace Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

public sealed class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
