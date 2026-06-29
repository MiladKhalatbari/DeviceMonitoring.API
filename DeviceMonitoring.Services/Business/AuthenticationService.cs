using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects.Authentication;
using DeviceMonitoring.Services.Exceptions;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DeviceMonitoring.Services.Business;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthenticationService(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> RegisterUserAsync(RegisterUserRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userName = request.UserName.Trim();
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        var existingUsers = await _userRepository.GetAllAsync(
            user => user.UserName == userName);

        if (existingUsers.Any())
        {
            throw new UserAlreadyExistsException();
        }

        var user = new User(
            userName: userName,
            firstName: firstName,
            lastName: lastName);

        user.SetPasswordHash(
            _passwordHasher.HashPassword(user, request.Password));

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task<User> ValidateUserCredentials(AuthenticationRequestBodyDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userName = request.UserName?.Trim();

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrEmpty(request.Password))
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        var users = await _userRepository.GetAllAsync(
            user => user.UserName == userName);

        var user = users.SingleOrDefault() ?? throw new UnauthorizedAccessException("Invalid username or password.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        return user;
    }
}