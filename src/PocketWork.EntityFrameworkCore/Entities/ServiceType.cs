namespace Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

public sealed class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Price { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
}
