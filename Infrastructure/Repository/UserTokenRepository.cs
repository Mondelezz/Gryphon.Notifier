using Domain.Models;
using Domain.Interfaces;

namespace Infrastructure.Repository;

internal class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
{
}
