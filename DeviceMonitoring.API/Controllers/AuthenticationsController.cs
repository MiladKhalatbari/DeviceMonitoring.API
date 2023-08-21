using DeviceMonitoring.Services;
using DeviceMonitoring.Services.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeviceMonitoring.API.Controllers
{

    public class AuthenticationsController : BaseController
    {
        private readonly DeviceMonitoring.Services.IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        public AuthenticationsController(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequestBodyDto userNamePassword)
        {
            var user = await _authenticationService.ValidateUserCredentials(userNamePassword);

            var issuer = _configuration["Authentication:Jwt:Issuer"];
            var audience = _configuration["Authentication:Jwt:Audience"];
            var secretKey = _configuration["Authentication:Jwt:SecretForKey"];

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName), 
            }; 

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(tokenString);
        }

    }
}
