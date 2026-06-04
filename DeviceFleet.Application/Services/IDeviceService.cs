using DeviceFleet.Application.Models;

namespace DeviceFleet.Application.Services;

/// <summary>Wynik operacji tworzenia urządzenia.</summary>
public enum CreateResult { Success, DuplicateSerialNumber }

public interface IDeviceService
{
    Task<IReadOnlyList<DeviceDto>> GetAllAsync(CancellationToken ct = default);
    Task<DeviceDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(CreateResult Result, DeviceDto? Device)> CreateAsync(CreateDeviceRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, UpdateDeviceRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
