namespace Application.Features.AuthorizationFeatures.Command;

public static partial class LoginWithGoogle
{
    public record ResponseDto(UserDto UserDto);

    public record UserDto(
        long Id,
        string Email,
        string Username,
        string? FirstName,
        string? LastName);
}
