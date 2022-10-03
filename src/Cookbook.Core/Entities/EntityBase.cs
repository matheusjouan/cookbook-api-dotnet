namespace Cookbook.Core.Entities;

public abstract class EntityBase
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
