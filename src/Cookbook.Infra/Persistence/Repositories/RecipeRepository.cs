using Cookbook.Core.Entities;
using Cookbook.Core.Enum;
using Cookbook.Core.Interfaces;
using Cookbook.Infra.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infra.Persistence.Repositories;
public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
{
    private readonly CookbookContext _context;

    public RecipeRepository(CookbookContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<Recipe>> GetAllRecipesUser(long userId, CategoryEnum? category, string theme)
    {
        IQueryable<Recipe> query = _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => r.UserId == userId);

        if (category.HasValue)
            query = query.Where(r => r.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(theme))
            query = query.Where(r => r.Title.ToLower().Contains(theme.ToLower())
                || r.Ingredients.Any(i => i.Product.ToLower().Contains(theme)));

        return await query.AsNoTracking()
            .OrderBy(r => r.Title)
            .ToListAsync();
    }

    public async Task<Recipe> GetRecipeById(long id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
