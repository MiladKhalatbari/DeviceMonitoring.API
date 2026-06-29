namespace DeviceMonitoring.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    public string UserName { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    private User(){}

    public User(string userName, string firstName, string lastName)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
