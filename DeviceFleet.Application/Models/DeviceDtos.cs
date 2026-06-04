using System.ComponentModel.DataAnnotations;
using DeviceFleet.Domain;

namespace DeviceFleet.Application.Models;

/// <summary>Reprezentacja urządzenia zwracana przez API (oddzielona od encji domenowej).</summary>
public record DeviceDto(Guid Id, string Name, string SerialNumber, DeviceStatus Status, DateTime? LastSeenUtc)
{
    public static DeviceDto FromEntity(Device d) =>
        new(d.Id, d.Name, d.SerialNumber, d.Status, d.LastSeenUtc);
}

/// <summary>Dane do utworzenia nowego urządzenia.</summary>
public record CreateDeviceRequest(
    [Required, MaxLength(200)] string Name,
    [Required, MaxLength(100)] string SerialNumber,
    DeviceStatus Status = DeviceStatus.Unknown);

/// <summary>Dane do aktualizacji istniejącego urządzenia.</summary>
public record UpdateDeviceRequest(
    [Required, MaxLength(200)] string Name,
    [Required, MaxLength(100)] string SerialNumber,
    DeviceStatus Status);
