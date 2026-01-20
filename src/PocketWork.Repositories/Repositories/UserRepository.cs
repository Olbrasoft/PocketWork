using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;
using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;
using Olbrasoft.PocketWork.Repositories.DTOs.Users;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Repositories.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PocketWorkDbContext _context;

    public UserRepository(PocketWorkDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : MapToResponseDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(MapToResponseDto);
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto createDto)
    {
        var user = new User
        {
            Name = createDto.Name,
            Surname = createDto.Surname,
            PhoneNumber = createDto.PhoneNumber,
            Email = createDto.Email,
            JobType = createDto.JobType
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToResponseDto(user);
    }

    public async Task UpdateAsync(int id, UpdateUserDto updateDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        user.Name = updateDto.Name;
        user.Surname = updateDto.Surname;
        user.PhoneNumber = updateDto.PhoneNumber;
        user.Email = updateDto.Email;
        user.JobType = updateDto.JobType;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersByJobTypeAsync(JobType jobType)
    {
        var users = await _context.Users
            .Where(u => u.JobType == jobType)
            .ToListAsync();

        return users.Select(MapToResponseDto);
    }

    private static UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            JobType = user.JobType
        };
    }
}
