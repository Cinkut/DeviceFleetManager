using DeviceFleet.Domain;

namespace DeviceFleet.Application.Abstractions;

/// <summary>
/// Abstrakcja dostępu do danych urządzeń. Warstwa Application zależy od tego
/// interfejsu, a nie od konkretnej technologii bazy danych (dependency inversion).
/// </summary>
public interface IDeviceRepository
{
    Task<IReadOnlyList<Device>> GetAllAsync(CancellationToken ct = default);
    Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> SerialNumberExistsAsync(string serialNumber, CancellationToken ct = default);
    Task AddAsync(Device device, CancellationToken ct = default);
    Task UpdateAsync(Device device, CancellationToken ct = default);
    Task DeleteAsync(Device device, CancellationToken ct = default);
}
