using Microsoft.AspNetCore.Mvc;
using Olbrasoft.PocketWork.Repositories.DTOs.ServiceTypes;
using Olbrasoft.PocketWork.Repositories.Interfaces;

namespace Olbrasoft.PocketWork.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceTypesController : ControllerBase
{
    private readonly IServiceTypeRepository _serviceTypeRepository;
    private readonly ILogger<ServiceTypesController> _logger;

    public ServiceTypesController(IServiceTypeRepository serviceTypeRepository, ILogger<ServiceTypesController> logger)
    {
        _serviceTypeRepository = serviceTypeRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceTypeResponseDto>>> GetAllServiceTypes()
    {
        var serviceTypes = await _serviceTypeRepository.GetAllAsync();
        return Ok(serviceTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceTypeResponseDto>> GetServiceType(int id)
    {
        var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
        if (serviceType == null)
        {
            return NotFound();
        }
        return Ok(serviceType);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceTypeResponseDto>> CreateServiceType(CreateServiceTypeDto createDto)
    {
        var created = await _serviceTypeRepository.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetServiceType), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServiceType(int id, UpdateServiceTypeDto updateDto)
    {
        try
        {
            await _serviceTypeRepository.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceType(int id)
    {
        await _serviceTypeRepository.DeleteAsync(id);
        return NoContent();
    }
}
