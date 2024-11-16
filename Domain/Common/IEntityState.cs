using Domain.Enums;

namespace Domain.Common;

public interface IEntityState
{
    public State State { get; set; }
}
