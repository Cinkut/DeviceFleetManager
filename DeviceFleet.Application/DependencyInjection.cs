using DeviceFleet.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceFleet.Application;

/// <summary>Rejestracja usług warstwy Application w kontenerze DI.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
        return services;
    }
}
