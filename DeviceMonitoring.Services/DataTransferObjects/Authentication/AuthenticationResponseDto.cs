namespace DeviceMonitoring.Services.DataTransferObjects.Authentication;
public sealed record AuthenticationResponseDto(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAtUtc);
