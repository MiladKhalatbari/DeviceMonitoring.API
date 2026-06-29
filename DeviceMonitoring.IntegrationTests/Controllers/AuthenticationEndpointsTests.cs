using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.IntegrationTests.Infrastructure;
using DeviceMonitoring.Services.DataTransferObjects.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceMonitoring.IntegrationTests.Controllers;

[Collection(SqlServerIntegrationTestCollection.NAME)]
public sealed class AuthenticationEndpointsTests : IAsyncLifetime
{
    private readonly DeviceMonitoringWebApplicationFactory _factory;
    private HttpClient _client = null!;

    public AuthenticationEndpointsTests(SqlServerContainerFixture sqlServerContainerFixture)
    {
        _factory = new DeviceMonitoringWebApplicationFactory(
            sqlServerContainerFixture.ConnectionString,
            useTestAuthentication: false);
    }

    public async ValueTask InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public ValueTask DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();

        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturn201AndUserShouldBeAbleToLogin()
    {
        // Arrange
        var registrationRequest = CreateValidRegistrationRequest();

        // Act
        var registerResponse = await _client.PostAsJsonAsync(
            "/api/auth/register",
            registrationRequest,
            TestContext.Current.CancellationToken);

        // Assert
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var registeredUser = await registerResponse.Content.ReadFromJsonAsync<RegisteredUserDto>(
            TestContext.Current.CancellationToken);

        registeredUser.Should().NotBeNull();
        registeredUser!.Id.Should().BeGreaterThan(0);
        registeredUser.UserName.Should().Be(registrationRequest.UserName);
        registeredUser.FirstName.Should().Be(registrationRequest.FirstName);
        registeredUser.LastName.Should().Be(registrationRequest.LastName);

        // Act
        var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new AuthenticationRequestBodyDto
            {
                UserName = registrationRequest.UserName,
                Password = registrationRequest.Password
            },
            TestContext.Current.CancellationToken);

        // Assert
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authenticationResponse = await loginResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>(
            TestContext.Current.CancellationToken);

        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.TokenType.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.ExpiresAtUtc.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturn409ProblemDetails_WhenUserNameAlreadyExists()
    {
        // Arrange
        var request = new RegisterUserRequestDto
        {
            UserName = "admin",
            FirstName = "Another",
            LastName = "Administrator",
            Password = "IntegrationPass123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problemDetails = await ReadProblemDetailsAsync(response);

        problemDetails.Status.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturn400ValidationProblemDetails_WhenPasswordIsMissing()
    {
        // Arrange
        var requestWithoutPassword = new
        {
            userName = $"user{Guid.NewGuid():N}",
            firstName = "Integration",
            lastName = "Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            requestWithoutPassword,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
            TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey(nameof(RegisterUserRequestDto.Password));
        problemDetails.Extensions.Should().ContainKey("traceId");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturn200AndJwt_WhenAdminCredentialsAreValid()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new AuthenticationRequestBodyDto
            {
                UserName = "admin",
                Password = "123456"
            },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>(
            TestContext.Current.CancellationToken);

        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.TokenType.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.ExpiresAtUtc.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturn401ProblemDetails_WhenPasswordIsIncorrect()
    {
        // Arrange
        var registrationRequest = CreateValidRegistrationRequest();
        await RegisterUserAsync(registrationRequest);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new AuthenticationRequestBodyDto
            {
                UserName = registrationRequest.UserName,
                Password = "WrongPassword123!"
            },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var problemDetails = await ReadProblemDetailsAsync(response);

        problemDetails.Status.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturn401ProblemDetails_WhenPasswordIsMissing()
    {
        // Arrange
        var requestWithoutPassword = new
        {
            userName = "admin"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            requestWithoutPassword,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var problemDetails = await ReadProblemDetailsAsync(response);

        problemDetails.Status.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturn401_WhenJwtIsMissing()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/devices/1",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturn401_WhenJwtIsInvalid()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "invalid-token");

        // Act
        var response = await _client.GetAsync(
            "/api/devices/1",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturn200_WhenJwtIsValid()
    {
        // Arrange
        var registrationRequest = CreateValidRegistrationRequest();
        var accessToken = await RegisterAndLoginAsync(registrationRequest);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        // Act
        var response = await _client.GetAsync(
            "/api/devices/1",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var device = await response.Content.ReadFromJsonAsync<Device>(
            TestContext.Current.CancellationToken);

        device.Should().NotBeNull();
        device!.Id.Should().Be(1);
        device.Name.Should().Be("Device 1");
    }

    private async Task RegisterUserAsync(RegisterUserRequestDto request)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            request,
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private async Task<string> RegisterAndLoginAsync(RegisterUserRequestDto registrationRequest)
    {
        await RegisterUserAsync(registrationRequest);

        var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new AuthenticationRequestBodyDto
            {
                UserName = registrationRequest.UserName,
                Password = registrationRequest.Password
            },
            TestContext.Current.CancellationToken);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authenticationResponse = await loginResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>(
            TestContext.Current.CancellationToken);

        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();

        return authenticationResponse.AccessToken;
    }

    private static RegisterUserRequestDto CreateValidRegistrationRequest()
    {
        return new RegisterUserRequestDto
        {
            UserName = $"user{Guid.NewGuid():N}",
            FirstName = "Integration",
            LastName = "Test",
            Password = "IntegrationPass123!"
        };
    }

    private static async Task<ProblemDetails> ReadProblemDetailsAsync(HttpResponseMessage response)
    {
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(
            TestContext.Current.CancellationToken);

        problemDetails.Should().NotBeNull();
        problemDetails!.Extensions.Should().ContainKey("traceId");

        return problemDetails;
    }
}