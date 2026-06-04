using DeviceFleet.Application.Models;
using DeviceFleet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(IDeviceService service) : ControllerBase
{
    /// <summary>Zwraca listę wszystkich urządzeń.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceDto>>> GetAll(CancellationToken ct)
        => Ok(await service.GetAllAsync(ct));

    /// <summary>Zwraca pojedyncze urządzenie po Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceDto>> GetById(Guid id, CancellationToken ct)
    {
        var device = await service.GetByIdAsync(id, ct);
        return device is null ? NotFound() : Ok(device);
    }

    /// <summary>Tworzy nowe urządzenie.</summary>
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> Create(CreateDeviceRequest request, CancellationToken ct)
    {
        var (result, device) = await service.CreateAsync(request, ct);

        if (result == CreateResult.DuplicateSerialNumber)
            return Conflict($"Urządzenie o numerze seryjnym '{request.SerialNumber}' już istnieje.");

        return CreatedAtAction(nameof(GetById), new { id = device!.Id }, device);
    }

    /// <summary>Aktualizuje istniejące urządzenie.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateDeviceRequest request, CancellationToken ct)
        => await service.UpdateAsync(id, request, ct) ? NoContent() : NotFound();

    /// <summary>Usuwa urządzenie.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
