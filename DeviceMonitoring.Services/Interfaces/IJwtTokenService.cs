using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Authentication;

namespace DeviceMonitoring.Services.Interfaces;

public interface IJwtTokenService
{
    AuthenticationResponseDto CreateToken(User user);
}