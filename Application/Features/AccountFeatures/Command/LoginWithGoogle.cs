using System.Security.Claims;

using Application.Common;

using Mediator;

using Microsoft.AspNetCore.Identity;

namespace Application.Features.AccountFeatures.Command;

/// <summary>
/// Вход в аккаунт через Google
/// </summary>
public static class LoginWithGoogle
{
    public record Command(
        ClaimsPrincipal? ClaimsPrincipal) : ICommandRequest<long>;

    public class Handler(UserManager<User> userManager) : IRequestHandler<Command, long>
    {
        private const string ProviderName = "Google";

        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = request.ClaimsPrincipal ??
                throw new ExternalLoginProviderException(ProviderName, "ClaimsPrincipal is null");

            string email = claims.FindFirstValue(ClaimTypes.Email) ??
                throw new ExternalLoginProviderException(ProviderName, "Email is null");

            User user = await userManager.FindByEmailAsync(email) ??
                await CreateAsync(claims, email);

            UserLoginInfo info = new(
                ProviderName,
                request.ClaimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                ProviderName);

            bool loginProviderExists = await LoginProviderExistsAsync(info.LoginProvider, info.ProviderKey);

            if (!loginProviderExists)
            {
                IdentityResult loginResult = await userManager.AddLoginAsync(user, info);

                if (!loginResult.Succeeded)
                {
                    throw new ExternalLoginProviderException(ProviderName,
                        $"Unable to login user: {string.Join(", ", loginResult.Errors)}");
                }
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
                throw new ExternalLoginProviderException(ProviderName,
                    $"Unable to create user: {string.Join(", ", result.Errors)}");
            }

            return newUser;
        }

        private async Task<bool> LoginProviderExistsAsync(string loginProvider, string providerKey)
        {
            User? user = await userManager.FindByLoginAsync(loginProvider, providerKey);
            return user != null;
        }
    }
}
