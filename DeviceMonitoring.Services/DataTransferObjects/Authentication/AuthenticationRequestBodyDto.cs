using System.ComponentModel.DataAnnotations;
namespace DeviceMonitoring.Services.DataTransferObjects.Authentication;
public record AuthenticationRequestBodyDto
{
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [MaxLength(256)]
    public string Password { get; set; } = string.Empty;
}
