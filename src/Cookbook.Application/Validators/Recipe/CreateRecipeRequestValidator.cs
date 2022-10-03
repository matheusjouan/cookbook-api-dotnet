using Cookbook.Application.Request.Recipe;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Cookbook.Application.Validators.Recipe;
public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
	public CreateRecipeRequestValidator()
	{
		RuleFor(x => x.Title).NotEmpty();
		RuleFor(x => x.Category).IsInEnum();
		RuleFor(x => x.PreparationMode).NotEmpty();
		RuleFor(x => x.Ingredients).NotEmpty();

		// Validação para a Tabela que se relaciona => RuleForEach pois é um ICollection
		RuleForEach(x => x.Ingredients).ChildRules(i =>
		{
			i.RuleFor(x => x.Product).NotEmpty();
			i.RuleFor(x => x.Product).NotEmpty();
		});

		RuleFor(x => x.Ingredients).Custom((ingredients, context) =>
		{
			var distinctProduct = ingredients.Select(i => i.Product).Distinct();
			if (distinctProduct.Count() != ingredients.Count())
			{
				context.AddFailure(new FluentValidation.Results.ValidationFailure("ingredients", ""));
			}
		});
	}
}
