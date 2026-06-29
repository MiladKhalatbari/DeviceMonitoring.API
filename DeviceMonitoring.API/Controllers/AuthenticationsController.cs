using DeviceMonitoring.Services.DataTransferObjects.Authentication;
using DeviceMonitoring.Services.Exceptions;
using DeviceMonitoring.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.Controllers;

[Route("api/auth")]
public class AuthenticationController(IAuthenticationService authenticationService, IJwtTokenService jwtTokenService) : BaseController
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthenticationResponseDto>> LoginAsync(
        [FromBody] AuthenticationRequestBodyDto request)
    {
        var user = await authenticationService.ValidateUserCredentials(request);

        var response = jwtTokenService.CreateToken(user);

        return Ok(response);

    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisteredUserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisteredUserDto>> RegisterAsync(
        [FromBody] RegisterUserRequestDto request)
    {

        var user = await authenticationService.RegisterUserAsync(request);

        var response = new RegisteredUserDto(
            user.Id,
            user.UserName,
            user.FirstName,
            user.LastName);

        return StatusCode(StatusCodes.Status201Created, response);

    }
}