namespace Olbrasoft.PocketWork.Repositories.DTOs.ServiceTypes;

public record ServiceTypeResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? Price { get; init; }
    public int? MinPrice { get; init; }
    public int? MaxPrice { get; init; }
}
