namespace DeviceMonitoring.Services.Exceptions;

public sealed class UserAlreadyExistsException : Exception

{
    public UserAlreadyExistsException()
        : base("A user with this username already exists.")
    {
    }
}