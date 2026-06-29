using System.ComponentModel.DataAnnotations;
namespace DeviceMonitoring.Services.Configuration;

public sealed class JwtOptions
{
    public const string SECTION_NAME = "Authentication:Jwt";

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Required]
    public string SecretForKey { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int ExpiryMinutes { get; init; } = 60;
}
