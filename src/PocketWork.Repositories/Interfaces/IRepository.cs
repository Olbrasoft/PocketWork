namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface IRepository<TEntity, TCreateDto, TUpdateDto, TResponseDto>
    where TEntity : class
{
    Task<TResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<TResponseDto>> GetAllAsync();
    Task<TResponseDto> CreateAsync(TCreateDto createDto);
    Task UpdateAsync(int id, TUpdateDto updateDto);
    Task DeleteAsync(int id);
}
