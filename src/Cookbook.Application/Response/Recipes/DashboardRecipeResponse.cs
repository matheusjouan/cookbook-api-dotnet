namespace Cookbook.Application.Response.Recipes;
public class DashboardRecipeResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public int IngredientsAmount { get; set; }
    public int PreparationTime { get; set; }
}
