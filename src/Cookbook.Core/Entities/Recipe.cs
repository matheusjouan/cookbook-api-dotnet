using Cookbook.Core.Enum;

namespace Cookbook.Core.Entities;
public class Recipe : EntityBase
{
    public string Title { get; set; }
    public CategoryEnum Category { get; set; }
    public string PreparationMode { get; set; }
    public int PreparationTime { get; set; }
    public ICollection<Ingredients> Ingredients { get; set; }
    public long UserId { get; set; }
}
