using System.ComponentModel.DataAnnotations;

namespace DeviceMonitoring.Services.DataTransferObjects.Authentication;
public sealed class RegisterUserRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 12)]
    public string Password { get; init; } = string.Empty;
}
