using DeviceFleet.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    // Tymczasowe dane w pamięci — w Etapie 2 zastąpimy je bazą (EF Core + PostgreSQL).
    private static readonly List<Device> Devices =
    [
        new Device { Name = "Radiotelefon-Krakow-01", SerialNumber = "SN-1001", Status = DeviceStatus.Online,  LastSeenUtc = DateTime.UtcNow.AddMinutes(-2) },
        new Device { Name = "Radiotelefon-Krakow-02", SerialNumber = "SN-1002", Status = DeviceStatus.Offline, LastSeenUtc = DateTime.UtcNow.AddHours(-5) },
        new Device { Name = "Terminal-Warszawa-01",   SerialNumber = "SN-2001", Status = DeviceStatus.Provisioning }
    ];

    /// <summary>Zwraca listę wszystkich urządzeń.</summary>
    [HttpGet]
    public ActionResult<IEnumerable<Device>> GetAll() => Ok(Devices);

    /// <summary>Zwraca pojedyncze urządzenie po Id.</summary>
    [HttpGet("{id:guid}")]
    public ActionResult<Device> GetById(Guid id)
    {
        var device = Devices.FirstOrDefault(d => d.Id == id);
        return device is null ? NotFound() : Ok(device);
    }
}
