namespace DeviceFleet.Api.Auth;

/// <summary>Konfiguracja tokenów JWT (sekcja "Jwt" w appsettings).</summary>
public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 60;
}
