using System.Security.Claims;
using System.Text;
using System.Transactions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

using Application.Features.AuthorizationFeatures.Command;
using Application.Interfaces;

using Domain.Interfaces;

using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace Application.Services;

public class AuthorizationService(
    IUserRepository userRepository,
    IUserLoginRepository userLoginRepository,
    IUserTokenRepository userTokenRepository) : IAuthorizationService
{
    private static class ProviderName
    {
        internal static string Google { get; } = "Google";
    }

    private sealed record AuthResult(string AccessToken, string? RefreshToken, DateTime ExpiresAtUtc);

    /// <summary>
    /// Аутентификация через Google
    /// </summary>
    /// <param name="authenticateResult">Содержит данные, которые вернул Google</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные о пользователе</returns>
    /// <exception cref="ExternalLoginProviderException"></exception>
    public async Task<LoginWithGoogle.ResponseDto> LoginWithGoogle(AuthenticateResult authenticateResult, CancellationToken cancellationToken)
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

        return new LoginWithGoogle.ResponseDto(Features.AuthorizationFeatures.Command.LoginWithGoogle.Mapper.Map(user));
    }

    /// <summary>
    /// Генерирует JWT Token
    /// </summary>
    /// <param name="issuer">Издатель токена</param>
    /// <param name="audience">Получатели, потребители токена</param>
    /// <param name="secret">Секрет</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="email">Почта</param>
    /// <param name="userName">Никнейм</param>
    /// <returns>JWT Token</returns>
    /// <exception cref="JwtTokenException"></exception>
    public string GenerateJwtToken(string issuer, string audience, string secret, long userId, string email, string userName)
    {
        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secret))
        {
            throw new JwtTokenException("JwtOptions is null");
        }
        Claim[] claims =
        [
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="email"></param>
    /// <param name="providerName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ExternalLoginProviderException"></exception>
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

    /// <summary>
    /// Создаёт логин провайдер
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="providerName">Наименование провайдера</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Логин-провайдер</returns>
    /// <exception cref="ExternalLoginProviderException"></exception>
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

    /// <summary>
    /// Добавляет токен доступа пользователя в бд
    /// </summary>
    /// <param name="authResult">Результат аутентификации</param>
    /// <param name="userLogin">Логин-провайдер</param>
    /// <param name="providerName">Наименование провайдера</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>UserToken</returns>
    /// <exception cref="ExternalLoginProviderException"></exception>
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

    /// <summary>
    /// Обновляет токен доступа
    /// </summary>
    /// <param name="authResult">Данные для авторизации</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userLoginId">Логин-провайдер пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
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

    /// <summary>
    /// Проверяет, был ли уже залогинен пользователь с таким логин-провайдером
    /// </summary>
    /// <param name="providerName">Логин-провайдер</param>
    /// <param name="email">Почта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>UserLogin</returns>
    private async Task<UserLogin?> LoginProviderExistsAsync(string providerName, string email, CancellationToken cancellationToken) =>
        await userLoginRepository.FindByProviderNameAsync(providerName, email, cancellationToken);

    /// <summary>
    /// Валидирует результат аутентификации
    /// </summary>
    /// <param name="authenticateResult">Результат аутентификации</param>
    /// <param name="providerName">Наименование провайдера</param>
    /// <returns>AuthResult</returns>
    /// <exception cref="ExternalLoginProviderException"></exception>
    private static AuthResult ValidateAuthResult(AuthenticateResult authenticateResult, string providerName)
    {
        string? accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
        string? refreshToken = authenticateResult.Properties?.GetTokenValue("refresh_token");
        DateTime expiresAtUtc = DateTime.Parse(authenticateResult.Properties.GetTokenValue("expires_at"), new CultureInfo("en-US"));

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ExternalLoginProviderException(providerName, "Access token token is missing.");
        }

        return new AuthResult(accessToken, refreshToken, expiresAtUtc.ToUniversalTime());
    }
}
