using Application.Features.AuthorizationFeatures.Command;

using Microsoft.AspNetCore.Authentication;

namespace Application.Interfaces;

public interface IAuthorizationService
{
    public Task<LoginWithGoogle.ResponseDto> LoginWithGoogle(
        AuthenticateResult authenticateResult,
        CancellationToken cancellationToken);

    public string GenerateJwtToken(
        string issuer,
        string audience,
        string secret,
        long userId,
        string email,
        string userName);
}
