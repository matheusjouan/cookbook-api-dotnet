using Cookbook.Core.Entities;
using Cookbook.Core.Enum;

namespace Cookbook.Core.Interfaces;
public interface IRecipeRepository : IBaseRepository<Recipe>
{
    Task<Recipe> GetRecipeById(long id);
    Task<IList<Recipe>> GetAllRecipesUser(long userId, CategoryEnum? category, string theme);
}
