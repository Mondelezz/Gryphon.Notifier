using Riok.Mapperly.Abstractions;

namespace Application.Features.AuthorizationFeatures.Command;

public static partial class LoginWithGoogle
{
    [Mapper]
    public static partial class Mapper
    {
        public static UserDto Map(User source) => new(
                source.Id,
                source.Email,
                source.Username,
                source.FirstName,
                source.LastName);
    }
}
