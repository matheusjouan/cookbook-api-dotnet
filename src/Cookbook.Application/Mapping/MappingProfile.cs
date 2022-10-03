using AutoMapper;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Request.Recipe;
using Cookbook.Application.Response.Recipes;
using Cookbook.Application.Response.User;
using Cookbook.Core.Entities;

namespace Cookbook.Application.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<User, CreateUserRequest>()
			.ForMember(u => u.Password, map => map.Ignore())
			.ReverseMap();

		CreateMap<User, UserAuthenticatedResponse>().ReverseMap();

		CreateMap<Recipe, CreateRecipeRequest>().ReverseMap();
        CreateMap<Recipe, UpdateRecipeRequest>().ReverseMap();
		CreateMap<Recipe, RecipeResponse>().ReverseMap();

        CreateMap<Ingredients, CreateIngredientsRequest>().ReverseMap();
        CreateMap<Ingredients, UpdateIngredientsRequest>().ReverseMap();
        CreateMap<Ingredients, IngredientResponse>().ReverseMap();

		CreateMap<Recipe, DashboardRecipeResponse>()
			.ForMember(dst => dst.IngredientsAmount, map => map.MapFrom(src => src.Ingredients.Count));
    }
}
