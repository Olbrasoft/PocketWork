using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;
using Olbrasoft.PocketWork.Repositories.DTOs.Users;

namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface IUserRepository : IRepository<User, CreateUserDto, UpdateUserDto, UserResponseDto>
{
    Task<IEnumerable<UserResponseDto>> GetUsersByJobTypeAsync(JobType jobType);
}
