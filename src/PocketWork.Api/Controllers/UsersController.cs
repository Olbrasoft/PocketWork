using Microsoft.AspNetCore.Mvc;
using Olbrasoft.PocketWork.EntityFrameworkCore.Enums;
using Olbrasoft.PocketWork.Repositories.DTOs.Users;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto createDto)
    {
        var created = await _userRepository.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateDto)
    {
        try
        {
            await _userRepository.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("jobtype/{jobType}")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersByJobType(JobType jobType)
    {
        var users = await _userRepository.GetUsersByJobTypeAsync(jobType);
        return Ok(users);
    }
}
