namespace Cookbook.Core.Entities;
public class Ingredients : EntityBase
{
    public string Product { get; set; }
    public string Amount { get; set; }
    public long RecipeId { get; set; }
}
