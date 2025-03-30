using Application.Features.AccountFeatures.Command;

using Domain.Models;

using Mediator;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(
    SignInManager<User> signManager,
    IMediator mediator) : ControllerBase
{
    [HttpGet("login/google")]
    public IResult SigninGoogle([FromQuery] string returnUrl)
    {
        string? redirectUri = Url.Action(nameof(GoogleLoginCallback), "Account", new { ReturnUrl = returnUrl });

        AuthenticationProperties authProps = signManager.ConfigureExternalAuthenticationProperties("Google", redirectUri);

        // Сообщает middleware, что нужно запустить процесс входа с помощью Google,
        // после ввода данных поьзователь будет перенаправлен на GoogleLoginCallback
        return Results.Challenge(authProps, ["Google"]);
    }

    [HttpGet("login/google/callback")]
    [ActionName("GoogleLoginCallback")]
    public async Task<IResult> GoogleLoginCallback([FromQuery] string returnUrl)
    {
        AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Results.Unauthorized();
        }

        await mediator.Send(new LoginWithGoogle.Command(authenticateResult.Principal));

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
