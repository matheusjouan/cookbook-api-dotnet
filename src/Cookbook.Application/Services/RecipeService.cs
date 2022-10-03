using AutoMapper;
using Cookbook.Application.Request.Recipe;
using Cookbook.Application.Response.Recipes;
using Cookbook.Application.Services.Interfaces;
using Cookbook.Core.Entities;
using Cookbook.Core.Excpetions;
using Cookbook.Core.Interfaces;
using System.Security.Authentication;

namespace Cookbook.Application.Services;
public class RecipeService : IRecipeService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public RecipeService(IUnitOfWork uow, IMapper mapper, IUserService userService)
    {
        _uow = uow;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<RecipeResponse> CreateRecipeAsync(CreateRecipeRequest request)
    {
        var user = await _userService.GetUserAuthenticatedInfoAsync();

        var recipe = _mapper.Map<Recipe>(request);
        recipe.UserId = user.Id;

        _uow.RecipeRepository.Add(recipe);
        await _uow.SaveChangeAsync();

        return _mapper.Map<RecipeResponse>(recipe);
    }

    public async Task<DashboardRecipeListResponse> GetRecipesInDashboardAsync(RecipesDashboardRequest request)
    {
        var user = await _userService.GetUserAuthenticatedInfoAsync();

        var recipes = await _uow.RecipeRepository.GetAllRecipesUser(user.Id, 
            request.Category, request.SearchTheme);

        return new DashboardRecipeListResponse
        {
            Recipes = _mapper.Map<List<DashboardRecipeResponse>>(recipes)
        };
    }

    public async Task<RecipeResponse> GetRecipeByIdAsync(long id)
    {
        var recipe = await _uow.RecipeRepository.GetRecipeById(id);

        if (recipe == null)
            throw new RecipeNotFoundException();

        return _mapper.Map<RecipeResponse>(recipe);
    }

    public async Task UpdateRecipeAsync(long id, UpdateRecipeRequest request)
    {
        var user = await _userService.GetUserAuthenticatedInfoAsync();

        var recipe = await _uow.RecipeRepository.GetRecipeById(id);

        if (recipe == null || recipe.UserId != user.Id)
            throw new RecipeNotFoundException();

        // Faz a atualização através do mapeamento
        _mapper.Map(request, recipe);

        _uow.RecipeRepository.Update(recipe);
        await _uow.SaveChangeAsync();
    }

    public async Task DeleteRecipeAsync(long id)
    {
        var user = await _userService.GetUserAuthenticatedInfoAsync();

        var recipe = await _uow.RecipeRepository.GetRecipeById(id);

        if (recipe == null || recipe.UserId != user.Id)
            throw new RecipeNotFoundException();

        _uow.RecipeRepository.Delete(recipe);
        await _uow.SaveChangeAsync();
    }
}
