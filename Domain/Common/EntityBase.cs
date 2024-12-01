namespace Domain.Common;

public class EntityBase : IEntityId, IEntityDate
{
    public long Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
