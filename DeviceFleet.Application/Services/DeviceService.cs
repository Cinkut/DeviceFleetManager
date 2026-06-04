using DeviceFleet.Application.Abstractions;
using DeviceFleet.Application.Models;
using DeviceFleet.Domain;

namespace DeviceFleet.Application.Services;

/// <summary>
/// Logika biznesowa operacji na urządzeniach. Korzysta z abstrakcji repozytorium,
/// dzięki czemu nie zależy od EF Core ani od konkretnej bazy danych.
/// </summary>
public class DeviceService(IDeviceRepository repository) : IDeviceService
{
    public async Task<IReadOnlyList<DeviceDto>> GetAllAsync(CancellationToken ct = default)
    {
        var devices = await repository.GetAllAsync(ct);
        return devices.Select(DeviceDto.FromEntity).ToList();
    }

    public async Task<DeviceDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var device = await repository.GetByIdAsync(id, ct);
        return device is null ? null : DeviceDto.FromEntity(device);
    }

    public async Task<(CreateResult Result, DeviceDto? Device)> CreateAsync(
        CreateDeviceRequest request, CancellationToken ct = default)
    {
        // Reguła biznesowa: numer seryjny musi być unikalny.
        if (await repository.SerialNumberExistsAsync(request.SerialNumber, ct))
            return (CreateResult.DuplicateSerialNumber, null);

        var device = new Device
        {
            Name = request.Name,
            SerialNumber = request.SerialNumber,
            Status = request.Status
        };

        await repository.AddAsync(device, ct);
        return (CreateResult.Success, DeviceDto.FromEntity(device));
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateDeviceRequest request, CancellationToken ct = default)
    {
        var device = await repository.GetByIdAsync(id, ct);
        if (device is null) return false;

        device.Name = request.Name;
        device.SerialNumber = request.SerialNumber;
        device.Status = request.Status;

        await repository.UpdateAsync(device, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var device = await repository.GetByIdAsync(id, ct);
        if (device is null) return false;

        await repository.DeleteAsync(device, ct);
        return true;
    }
}
