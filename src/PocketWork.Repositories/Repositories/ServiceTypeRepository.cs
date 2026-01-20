using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.ServiceTypes;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Repositories.Repositories;

public class ServiceTypeRepository : IServiceTypeRepository
{
    private readonly PocketWorkDbContext _context;

    public ServiceTypeRepository(PocketWorkDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceTypeResponseDto?> GetByIdAsync(int id)
    {
        var serviceType = await _context.ServiceTypes.FindAsync(id);
        return serviceType == null ? null : MapToResponseDto(serviceType);
    }

    public async Task<IEnumerable<ServiceTypeResponseDto>> GetAllAsync()
    {
        var serviceTypes = await _context.ServiceTypes.ToListAsync();
        return serviceTypes.Select(MapToResponseDto);
    }

    public async Task<ServiceTypeResponseDto> CreateAsync(CreateServiceTypeDto createDto)
    {
        var serviceType = new ServiceType
        {
            Name = createDto.Name,
            Price = createDto.Price,
            MinPrice = createDto.MinPrice,
            MaxPrice = createDto.MaxPrice
        };

        _context.ServiceTypes.Add(serviceType);
        await _context.SaveChangesAsync();

        return MapToResponseDto(serviceType);
    }

    public async Task UpdateAsync(int id, UpdateServiceTypeDto updateDto)
    {
        var serviceType = await _context.ServiceTypes.FindAsync(id);
        if (serviceType == null)
        {
            throw new KeyNotFoundException($"ServiceType with id {id} not found");
        }

        serviceType.Name = updateDto.Name;
        serviceType.Price = updateDto.Price;
        serviceType.MinPrice = updateDto.MinPrice;
        serviceType.MaxPrice = updateDto.MaxPrice;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var serviceType = await _context.ServiceTypes.FindAsync(id);
        if (serviceType != null)
        {
            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();
        }
    }

    private static ServiceTypeResponseDto MapToResponseDto(ServiceType serviceType)
    {
        return new ServiceTypeResponseDto
        {
            Id = serviceType.Id,
            Name = serviceType.Name,
            Price = serviceType.Price,
            MinPrice = serviceType.MinPrice,
            MaxPrice = serviceType.MaxPrice
        };
    }
}
