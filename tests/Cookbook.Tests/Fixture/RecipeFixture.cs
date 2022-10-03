
using Bogus.DataSets;
using Cookbook.Application.Request.Recipe;
using Cookbook.Core.Entities;
using Cookbook.Core.Enum;

namespace Cookbook.Tests.Fixture;
public class RecipeFixture
{
    public CreateRecipeRequest GenerateRecipeRequest()
    {
        var recipe = new CreateRecipeRequest
        {
            Title = new Random().Next().ToString(),
            Category = CategoryEnum.Dinner,
            PreparationMode = new Random().Next().ToString(),
            Ingredients = new List<CreateIngredientsRequest>
            {
                new CreateIngredientsRequest { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() },
                new CreateIngredientsRequest { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() }
            }
        };

        return recipe;
    }

    public Recipe GenerateRecipe()
    {
        var recipe = new Recipe
        {
            Title = new Random().Next().ToString(),
            UserId = 5,
            Category = CategoryEnum.Dinner,
            PreparationMode = new Random().Next().ToString(),
            Ingredients = new List<Ingredients>
            {
                new Ingredients { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() },
                new Ingredients { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() }
            }
        };

        return recipe;
    }

    public UpdateRecipeRequest UpdateeRecipeRequest()
    {
        var recipe = new UpdateRecipeRequest
        {
            Title = new Random().Next().ToString(),
            Category = CategoryEnum.Dinner,
            PreparationMode = new Random().Next().ToString(),
            Ingredients = new List<UpdateIngredientsRequest>
            {
                new UpdateIngredientsRequest { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() },
                new UpdateIngredientsRequest { Product = new Random().Next().ToString(), Amount = new Random().Next().ToString() }
            }
        };

        return recipe;
    }
}
