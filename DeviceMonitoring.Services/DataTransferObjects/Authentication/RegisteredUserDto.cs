namespace DeviceMonitoring.Services.DataTransferObjects.Authentication;
public sealed record RegisteredUserDto(
    int Id,
    string UserName,
    string FirstName,
    string LastName);