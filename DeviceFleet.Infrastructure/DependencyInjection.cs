using DeviceFleet.Application.Abstractions;
using DeviceFleet.Infrastructure.Data;
using DeviceFleet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceFleet.Infrastructure;

/// <summary>Rejestracja usług warstwy Infrastructure (baza danych, repozytoria).</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        return services;
    }
}
