using Cookbook.Core.Enum;

namespace Cookbook.Application.Request.Recipe;
public class UpdateRecipeRequest
{
    public long Id { get; set; }
    public string Title { get; set; }
    public CategoryEnum Category { get; set; }
    public int PreparationTime { get; set; }

    public string PreparationMode { get; set; }
    public List<UpdateIngredientsRequest> Ingredients { get; set; }

    public UpdateRecipeRequest()
    {
        Ingredients = new List<UpdateIngredientsRequest>();
    }
}
