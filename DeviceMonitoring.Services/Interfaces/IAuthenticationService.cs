using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Authentication;

namespace DeviceMonitoring.Services.Interfaces;

public interface IAuthenticationService
{
    Task<User> RegisterUserAsync(RegisterUserRequestDto request);
    Task<User> ValidateUserCredentials(AuthenticationRequestBodyDto user);
}