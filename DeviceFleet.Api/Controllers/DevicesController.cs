using DeviceFleet.Api.Data;
using DeviceFleet.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceFleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(AppDbContext db) : ControllerBase
{
    /// <summary>Zwraca listę wszystkich urządzeń.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetAll()
        => Ok(await db.Devices.AsNoTracking().ToListAsync());

    /// <summary>Zwraca pojedyncze urządzenie po Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Device>> GetById(Guid id)
    {
        var device = await db.Devices.FindAsync(id);
        return device is null ? NotFound() : Ok(device);
    }

    /// <summary>Tworzy nowe urządzenie.</summary>
    [HttpPost]
    public async Task<ActionResult<Device>> Create(CreateDeviceRequest request)
    {
        var device = new Device
        {
            Name = request.Name,
            SerialNumber = request.SerialNumber,
            Status = request.Status
        };

        db.Devices.Add(device);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
    }

    /// <summary>Aktualizuje istniejące urządzenie.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateDeviceRequest request)
    {
        var device = await db.Devices.FindAsync(id);
        if (device is null) return NotFound();

        device.Name = request.Name;
        device.SerialNumber = request.SerialNumber;
        device.Status = request.Status;

        await db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Usuwa urządzenie.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var device = await db.Devices.FindAsync(id);
        if (device is null) return NotFound();

        db.Devices.Remove(device);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
