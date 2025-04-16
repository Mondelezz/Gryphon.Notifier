using System.Security.Claims;

using Application.Interfaces;

using Domain.Interfaces;

using Microsoft.AspNetCore.Authentication;

namespace Application.Services;

public class AuthorizationService(IUserRepository userRepository, IUserLoginRepository userLoginRepository) : IAuthorizationService
{
    internal static class ProviderName
    {
        internal static string Google { get; } = "Google";
    }

    public async Task<long> LoginWithGoogle(AuthenticateResult authenticateResult, CancellationToken cancellationToken)
    {
        ClaimsPrincipal claims = authenticateResult.Principal ??
                throw new ExternalLoginProviderException(ProviderName.Google, "ClaimsPrincipal is null");

        string email = claims.FindFirstValue(ClaimTypes.Email) ??
                throw new ExternalLoginProviderException(ProviderName.Google, "Email is null");

        User user = await userRepository.FindByEmailAsync(email, cancellationToken) ??
            await CreateUserAsync(claims, email, cancellationToken);

        bool loginProviderExists = await LoginProviderExistsAsync(ProviderName.Google, email, cancellationToken);

        if (!loginProviderExists)
        {
            await CreateUserLoginAsync(user, cancellationToken);
        }

        return user.Id;
    }

    private async Task<User> CreateUserAsync(ClaimsPrincipal claims, string email, CancellationToken cancellationToken)
    {
        User newUser = new()
        {
            FirstName = claims.FindFirstValue(ClaimTypes.GivenName),
            LastName = claims.FindFirstValue(ClaimTypes.Surname),
            Username = email,
            Email = email,
            IsEmailConfirmed = true,
        };

        bool result = await userRepository.CreateUserAsync(newUser, cancellationToken);

        if (!result)
        {
            throw new ExternalLoginProviderException(ProviderName.Google,
                $"Unable to create user with Email: {newUser.Email}");
        }

        return newUser;
    }

    private async Task<UserLogin> CreateUserLoginAsync(User user, CancellationToken cancellationToken)
    {
        UserLogin userLogin = new()
        {
            LoginProvider = ProviderName.Google,
            Email = user.Email,
            UserId = user.Id,
        };

        bool result = await userLoginRepository.CreateUserLoginAsync(userLogin, cancellationToken);

        if (!result)
        {
            throw new ExternalLoginProviderException(ProviderName.Google,
                $"Unable to create userLogin with Email: {userLogin.Email}");
        }

        return userLogin;
    }

    private async Task<UserToken> CreateOrUpdateUserTokenAsync(AuthenticateResult authenticateResult, long userId, CancellationToken cancellationToken)
    {
        string? accessToken = authenticateResult.Properties.GetTokenValue("access_token");
        string? refreshToken = authenticateResult.Properties.GetTokenValue("refresh_token");

        if (long.TryParse(authenticateResult.Properties.GetTokenValue("expires_in"), out long expiresAt) &&
            !string.IsNullOrEmpty(accessToken) &&
            !string.IsNullOrEmpty(refreshToken))
        {
            DateTime accessTokenExpiresAt = DateTime.UtcNow.AddSeconds(expiresAt);

            UserToken userToken = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                UserId = userId,
            };
        }
    }

    private async Task<bool> LoginProviderExistsAsync(string providerName, string email, CancellationToken cancellationToken)
    {
        UserLogin? userLogin = await userLoginRepository.FindByProviderNameAsync(providerName, email, cancellationToken);
        return userLogin != null;
    }
}
