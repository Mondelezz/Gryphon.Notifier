namespace Domain.Common;

public interface IEntityState
{
    /// <summary>
    /// Удалена
    /// </summary>
    public bool IsDeleted { get; set; }
}
