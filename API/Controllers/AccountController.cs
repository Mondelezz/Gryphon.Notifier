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
    LinkGenerator linkGenerator,
    SignInManager<User> signManager,
    IMediator mediator) : ControllerBase
{
    [HttpGet("login/google")]
    public IResult SigninGoogle([FromQuery] string returnUrl)
    {
        AuthenticationProperties authProp = signManager.ConfigureExternalAuthenticationProperties("Google",
            linkGenerator.GetPathByName(HttpContext, "GoogleLoginCallback")
            + $"?returnUrl={returnUrl}");

        // Сообщает middleware, что нужно запустить процесс входа с помощью Google,
        // после ввода данных поьзователь будет перенаправлен на GoogleLoginCallback
        return Results.Challenge(authProp, ["Google"]);
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
