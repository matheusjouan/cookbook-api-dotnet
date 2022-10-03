using Cookbook.Core.Enum;

namespace Cookbook.Application.Request.Recipe;
public class CreateRecipeRequest
{
    public string Title { get; set; }
    public CategoryEnum Category { get; set; }
    public int PreparationTime { get; set; }
    public string PreparationMode { get; set; }
    public List<CreateIngredientsRequest> Ingredients { get; set; }

    public CreateRecipeRequest()
    {
        Ingredients = new List<CreateIngredientsRequest>();
    }
}
