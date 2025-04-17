using System.Security.Claims;
using System.Transactions;

using Application.Interfaces;

using Domain.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Services;

public class AuthorizationService(
    IUserRepository userRepository,
    IUserLoginRepository userLoginRepository,
    IUserTokenRepository userTokenRepository) : IAuthorizationService
{
    internal static class ProviderName
    {
        internal static string Google { get; } = "Google";
    }

    internal record AuthResult(
        string AccessToken,
        string? RefreshToken,
        DateTime ExpiresAtUtc);

    public async Task<long> LoginWithGoogle(AuthenticateResult authenticateResult, CancellationToken cancellationToken)
    {
        AuthResult authResult = ValidateAuthResult(authenticateResult, ProviderName.Google);

        ClaimsPrincipal claims = authenticateResult.Principal ??
                throw new ExternalLoginProviderException(ProviderName.Google, "ClaimsPrincipal is null");

        string email = claims.FindFirstValue(ClaimTypes.Email) ??
                throw new ExternalLoginProviderException(ProviderName.Google, "Email is null");

        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

        User user = await userRepository.FindByEmailAsync(email, cancellationToken)
                ?? await CreateUserAsync(claims, email, ProviderName.Google, cancellationToken);

        UserLogin? userLogin = await LoginProviderExistsAsync(ProviderName.Google, user.Email, cancellationToken);

        if (userLogin is null)
        {
            userLogin = await CreateUserLoginAsync(user, ProviderName.Google, cancellationToken);
            await CreateUserTokenAsync(authResult, userLogin, ProviderName.Google, cancellationToken);
        }
        else
        {
            await UpdateUserTokenAsync(authResult, user.Id, userLogin.Id, cancellationToken);
        }

        scope.Complete();

        return user.Id;
    }

    private async Task<User> CreateUserAsync(
        ClaimsPrincipal claims,
        string email,
        string providerName,
        CancellationToken cancellationToken)
    {
        User newUser = new()
        {
            FirstName = claims.FindFirstValue(ClaimTypes.GivenName),
            LastName = claims.FindFirstValue(ClaimTypes.Surname),
            Username = email,
            Email = email,
            IsEmailConfirmed = true,
        };

        bool isCreated = await userRepository.CreateUserAsync(newUser, cancellationToken);

        if (!isCreated)
        {
            throw new ExternalLoginProviderException(providerName,
                $"Unable to create user with Email: {newUser.Email}");
        }

        return newUser;
    }

    private async Task<UserLogin> CreateUserLoginAsync(User user, string providerName, CancellationToken cancellationToken)
    {
        UserLogin userLogin = new()
        {
            LoginProvider = providerName,
            Email = user.Email,
            UserId = user.Id,
        };

        bool isCreated = await userLoginRepository.CreateUserLoginAsync(userLogin, cancellationToken);

        if (!isCreated)
        {
            throw new ExternalLoginProviderException(providerName,
                $"Unable to create userLogin with Email: {userLogin.Email}");
        }

        return userLogin;
    }

    private async Task<UserToken> CreateUserTokenAsync(
        AuthResult authResult,
        UserLogin userLogin,
        string providerName,
        CancellationToken cancellationToken)
    {
        UserToken userToken = new()
        {
            UserId = userLogin.UserId,
            AccessToken = authResult.AccessToken,
            RefreshToken = authResult.RefreshToken ?? string.Empty,
            AccessTokenExpiresAt = authResult.ExpiresAtUtc,
            UserLogin = userLogin,
            UserLoginId = userLogin.Id
        };

        bool isCreated = await userTokenRepository.CreateUserTokenAsync(userToken, cancellationToken);
        if (!isCreated)
        {
            throw new ExternalLoginProviderException(providerName,
                $"Failed to create or update user token for UserId: {userLogin.UserId}.");
        }

        return userToken;
    }

    private async Task<UserToken> UpdateUserTokenAsync(
       AuthResult authResult,
       long userId,
       long userLoginId,
       CancellationToken cancellationToken)
    {
        UserToken? userToken = await userTokenRepository.GetByUserAndLoginIdAsync(userId, userLoginId, cancellationToken)
            ?? throw new EntityNotFoundException(userId, userLoginId, "UserToken with UserId", "UserToken with UserLoginId");

        userToken.AccessToken = authResult.AccessToken;
        userToken.AccessTokenExpiresAt = authResult.ExpiresAtUtc;

        return await userTokenRepository.UpdateAsync(userToken, cancellationToken);
    }

    private async Task<UserLogin?> LoginProviderExistsAsync(string providerName, string email, CancellationToken cancellationToken) => await
        userLoginRepository.FindByProviderNameAsync(providerName, email, cancellationToken);

    private AuthResult ValidateAuthResult(AuthenticateResult authenticateResult, string providerName)
    {
        string? accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
        string? refreshToken = authenticateResult.Properties?.GetTokenValue("refresh_token");
        string? expiresAt = authenticateResult.Properties?.GetTokenValue("expires_at");

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ExternalLoginProviderException(providerName, "Access token token is missing.");
        }

        if (!DateTime.TryParse(expiresAt, out DateTime expiresAtUtc))
        {
            throw new ExternalLoginProviderException(providerName, "Invalid or missing expires_at.");
        }

        return new AuthResult(accessToken, refreshToken, expiresAtUtc.ToUniversalTime());
    }
}
