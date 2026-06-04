using System.ComponentModel.DataAnnotations;

namespace DeviceFleet.Api.Models;

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
