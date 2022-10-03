using AutoMapper;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Request.Recipe;
using Cookbook.Application.Response.Recipes;
using Cookbook.Application.Response.User;
using Cookbook.Core.Entities;

namespace Cookbook.Tests.Configuration;

public class AutoMapperConfig
{
    public static IMapper GetConfiguration()
    {
        var autoMapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, CreateUserRequest>()
                .ForMember(u => u.Password, map => map.Ignore())
                .ReverseMap();

            cfg.CreateMap<User, UserAuthenticatedResponse>().ReverseMap();

            cfg.CreateMap<Recipe, CreateRecipeRequest>().ReverseMap();
            cfg.CreateMap<Recipe, RecipeResponse>().ReverseMap();
            cfg.CreateMap<Recipe, UpdateRecipeRequest>().ReverseMap();

            cfg.CreateMap<Ingredients, CreateIngredientsRequest>().ReverseMap();
            cfg.CreateMap<Ingredients, IngredientResponse>().ReverseMap();
            cfg.CreateMap<Ingredients, UpdateIngredientsRequest>().ReverseMap();
        });

        return autoMapperConfig.CreateMapper();
    }
}
