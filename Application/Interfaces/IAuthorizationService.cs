using Microsoft.AspNetCore.Authentication;

namespace Application.Interfaces;

public interface IAuthorizationService
{
    public Task<long> LoginWithGoogle(
        AuthenticateResult authenticateResult,
        CancellationToken cancellationToken);
}
