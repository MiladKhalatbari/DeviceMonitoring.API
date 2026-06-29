using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.Configuration;
using DeviceMonitoring.Services.DataTransferObjects.Authentication;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeviceMonitoring.Services.Business;

public class JwtTokenService(IOptions<JwtOptions> jwtOptions) : IJwtTokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public AuthenticationResponseDto CreateToken(User user)
    {
        var now = DateTime.UtcNow;
        var expiresAtUtc = now.AddMinutes(_jwtOptions.ExpiryMinutes);

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.SecretForKey));

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now,
            expires: expiresAtUtc,
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthenticationResponseDto(
            AccessToken: accessToken,
            TokenType: "Bearer",
            ExpiresAtUtc: expiresAtUtc);
    }
}
