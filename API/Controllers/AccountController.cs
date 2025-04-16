using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using IAuthorizationService = Application.Interfaces.IAuthorizationService;
namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(IAuthorizationService authorizationService) : ControllerBase
{
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

        await authorizationService.LoginWithGoogle(authenticateResult, cancellationToken);

        return Results.Redirect(returnUrl);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return Ok("Logged out");
    }
}
