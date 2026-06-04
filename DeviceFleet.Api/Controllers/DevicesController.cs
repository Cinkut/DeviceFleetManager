using DeviceFleet.Application.Models;
using DeviceFleet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // każdy endpoint wymaga uwierzytelnienia (zalogowania)
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

    /// <summary>Tworzy nowe urządzenie. Wymaga roli Admin.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DeviceDto>> Create(CreateDeviceRequest request, CancellationToken ct)
    {
        var (result, device) = await service.CreateAsync(request, ct);

        if (result == CreateResult.DuplicateSerialNumber)
            return Conflict($"Urządzenie o numerze seryjnym '{request.SerialNumber}' już istnieje.");

        return CreatedAtAction(nameof(GetById), new { id = device!.Id }, device);
    }

    /// <summary>Aktualizuje istniejące urządzenie. Wymaga roli Admin.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateDeviceRequest request, CancellationToken ct)
        => await service.UpdateAsync(id, request, ct) ? NoContent() : NotFound();

    /// <summary>Usuwa urządzenie. Wymaga roli Admin.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
