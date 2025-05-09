using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Application.Features.AuthorizationFeatures.Command;
using IAuthorizationService = Application.Interfaces.IAuthorizationService;
using API.Options;
using Microsoft.Extensions.Options;
namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly JwtOptions _jwtOptions;
    public AccountController(IAuthorizationService authorizationService, IOptions<JwtOptions> jwtOptions)
    {
        _authorizationService = authorizationService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpGet("login/google")]
    public IActionResult SigninGoogle([FromQuery] string returnUrl)
    {
        string? redirectUri = Url.Action(nameof(GoogleLoginCallback), "Account", new { ReturnUrl = returnUrl });

        AuthenticationProperties properties = new()
        {
            RedirectUri = redirectUri
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("login/google/callback")]
    public async Task<IActionResult> GoogleLoginCallback(string returnUrl, CancellationToken cancellationToken = default)
    {
        AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Redirect($"{returnUrl}?authFailed=true");
        }

        LoginWithGoogle.ResponseDto response = await _authorizationService.LoginWithGoogle(authenticateResult, cancellationToken);

        string authToken = _authorizationService.GenerateJwtToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            _jwtOptions.Secret,
            response.UserDto.Id,
            response.UserDto.Email,
            response.UserDto.Username);

        return Redirect($"{returnUrl}?authToken={authToken}");
    }

    [HttpGet("check-auth")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult CheckAuth()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok("Successful login");
        }
        return Unauthorized();
    }

    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [HttpPost("logout")]
    public IActionResult Logout() => Ok("Logged out");
}
