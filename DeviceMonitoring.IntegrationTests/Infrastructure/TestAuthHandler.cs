using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace DeviceMonitoring.IntegrationTests.Infrastructure;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SCHEME_NAME = "IntegrationTestScheme";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "integration-test-user"),
            new Claim(ClaimTypes.Name, "Integration Test User")
        };

        var identity = new ClaimsIdentity(claims, SCHEME_NAME);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SCHEME_NAME);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}