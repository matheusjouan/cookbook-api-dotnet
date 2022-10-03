using Microsoft.AspNetCore.Http;

namespace Cookbook.Core.Excpetions;
public class RecipeNotFoundException : ExceptionsBase
{
	public RecipeNotFoundException()
	{
		Error = new Error
		{
			Title = "RecipeNotFoundException",
			Status = StatusCodes.Status404NotFound,
			Detail = "Recipe does not exist"
		};
	}
}
