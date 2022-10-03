using Cookbook.Application.Request.Recipe;
using Cookbook.Application.Response.Recipes;

namespace Cookbook.Application.Services.Interfaces;
public interface IRecipeService
{
    Task<RecipeResponse> CreateRecipeAsync(CreateRecipeRequest request);

    Task<DashboardRecipeListResponse> GetRecipesInDashboardAsync(RecipesDashboardRequest request);

    Task<RecipeResponse> GetRecipeByIdAsync(long id);

    Task UpdateRecipeAsync(long id, UpdateRecipeRequest request);

    Task DeleteRecipeAsync(long id);
}
