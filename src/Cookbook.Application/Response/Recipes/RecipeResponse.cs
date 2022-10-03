using Cookbook.Application.Request.Recipe;
using Cookbook.Core.Enum;

namespace Cookbook.Application.Response.Recipes;
public class RecipeResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public CategoryEnum Category { get; set; }
    public string PreparationMode { get; set; }
    public int PreparationTime { get; set; }

    public List<IngredientResponse> Ingredients { get; set; }
}
