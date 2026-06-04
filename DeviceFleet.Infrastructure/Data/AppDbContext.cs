using DeviceFleet.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeviceFleet.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
            entity.Property(d => d.SerialNumber).IsRequired().HasMaxLength(100);
            entity.HasIndex(d => d.SerialNumber).IsUnique();
            entity.Property(d => d.Status).HasConversion<string>();
        });
    }
}
