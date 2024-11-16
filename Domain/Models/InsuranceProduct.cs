using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class InsuranceProduct : EntityBase, IEntityState
{
    public State State { get; set; }

}
