using DeviceFleet.Application.Abstractions;
using DeviceFleet.Domain;
using DeviceFleet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceFleet.Infrastructure.Repositories;

/// <summary>Implementacja repozytorium oparta o EF Core / PostgreSQL.</summary>
public class DeviceRepository(AppDbContext db) : IDeviceRepository
{
    public async Task<IReadOnlyList<Device>> GetAllAsync(CancellationToken ct = default)
        => await db.Devices.AsNoTracking().ToListAsync(ct);

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Devices.FindAsync([id], ct);

    public async Task<bool> SerialNumberExistsAsync(string serialNumber, CancellationToken ct = default)
        => await db.Devices.AnyAsync(d => d.SerialNumber == serialNumber, ct);

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        db.Devices.Add(device);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        db.Devices.Update(device);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Device device, CancellationToken ct = default)
    {
        db.Devices.Remove(device);
        await db.SaveChangesAsync(ct);
    }
}
