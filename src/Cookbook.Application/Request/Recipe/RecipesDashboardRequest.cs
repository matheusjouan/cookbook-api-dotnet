using Cookbook.Core.Enum;

namespace Cookbook.Application.Request.Recipe;
public class RecipesDashboardRequest
{
    public string SearchTheme { get; set; }
    public CategoryEnum? Category { get; set; }
}
