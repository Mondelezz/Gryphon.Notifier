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
    public async Task<IResult> GoogleLoginCallback([FromQuery] string returnUrl, CancellationToken cancellationToken = default)
    {
        AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Results.Unauthorized();
        }

        LoginWithGoogle.ResponseDto response = await _authorizationService.LoginWithGoogle(authenticateResult, cancellationToken);

        string token = _authorizationService.GenerateJwtToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            _jwtOptions.Secret,
            response.UserDto.Id,
            response.UserDto.Email,
            response.UserDto.Username);

        Response.Cookies.Append("Bearer_token", token, new CookieOptions
        {
            HttpOnly = true, // Запрещает доступ к куке через JavaScript (злоумышленник не сможет украсть токен через JS-код).
            Secure = true, // Куки отправляются только по HTTPS
            SameSite = SameSiteMode.None, // Защита от CSRF
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        });

        return Results.Redirect(returnUrl);
    }

    [HttpGet("check-auth")]
    public IActionResult CheckAuth()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok();
        }
        return Unauthorized();
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("Bearer_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok("Logged out");
    }
}
