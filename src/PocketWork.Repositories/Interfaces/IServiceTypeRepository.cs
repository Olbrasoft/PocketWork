using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.Repositories.DTOs.ServiceTypes;

namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface IServiceTypeRepository : IRepository<ServiceType, CreateServiceTypeDto, UpdateServiceTypeDto, ServiceTypeResponseDto>
{
}
