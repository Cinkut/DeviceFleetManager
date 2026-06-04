namespace DeviceFleet.Domain;

/// <summary>
/// Status urządzenia w systemie zarządzania flotą.
/// </summary>
public enum DeviceStatus
{
    Unknown = 0,
    Online = 1,
    Offline = 2,
    Provisioning = 3,
    Error = 4
}

/// <summary>
/// Encja domenowa: pojedyncze urządzenie zarządzane przez system.
/// </summary>
public class Device
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Czytelna nazwa urządzenia, np. "Radiotelefon-Kraków-01".</summary>
    public required string Name { get; set; }

    /// <summary>Unikalny numer seryjny.</summary>
    public required string SerialNumber { get; set; }

    public DeviceStatus Status { get; set; } = DeviceStatus.Unknown;

    /// <summary>Ostatni kontakt urządzenia z systemem (UTC).</summary>
    public DateTime? LastSeenUtc { get; set; }
}
