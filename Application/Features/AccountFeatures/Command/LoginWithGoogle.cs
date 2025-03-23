using System.Security.Claims;

using Application.Common;

using Mediator;

using Microsoft.AspNet.Identity;

namespace Application.Features.AccountFeatures.Command;

/// <summary>
/// Вход в аккаунт через Google
/// </summary>
public static class LoginWithGoogle
{
    public record Command(
        ClaimsPrincipal? ClaimsPrincipal) : ICommandRequest<string>;

    public class Handler(UserManager<User> userManager) : IRequestHandler<Command, string>
    {
        public async ValueTask<string> Handle(Command request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = request.ClaimsPrincipal ??
                throw new ExternalLoginProviderException("Google", "ClaimsPrincipal is null");

            string email = claims.FindFirstValue(ClaimTypes.Email) ??
                throw new ExternalLoginProviderException("Google", "Email is null");

            User user = await userManager.FindByEmailAsync(email) ??
                await CreateAsync(claims, email);

            UserLoginInfo info = new(
                "Google",
                request.ClaimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty);

            IdentityResult loginResult = await userManager.AddLoginAsync(user.Id, info);

            if (!loginResult.Succeeded)
            {
                throw new ExternalLoginProviderException("Google",
                    $"Unable to login user: {string.Join(", ", loginResult.Errors)}");
            }

            return user.Id;
        }

        /// <summary>
        /// Создаёт пользователя
        /// </summary>
        /// <param name="claims">Данные от Google</param>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        /// <exception cref="ExternalLoginProviderException">Ошибка внешнего провайдера аутентификации</exception>
        private async Task<User> CreateAsync(ClaimsPrincipal claims, string email)
        {
            User newUser = new()
            {
                Email = email,
                UserName = email,
                FirstName = claims.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                LastName = claims.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                throw new ExternalLoginProviderException("Google",
                    $"Unable to create user: {string.Join(", ", result.Errors)}");
            }

            return newUser;
        }
    }
}
