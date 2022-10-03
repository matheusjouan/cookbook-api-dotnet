using Cookbook.Application.Request.Recipe;
using Cookbook.Application.Response.Recipes;
using Cookbook.Application.Response.User;
using Cookbook.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status201Created)]

    public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeRequest request)
    {
        var recipe = await _recipeService.CreateRecipeAsync(request);
        return Created(string.Empty, recipe);
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardRecipeListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRecipesUserInDashboard([FromQuery] RecipesDashboardRequest request)
    {
        var recipes = await _recipeService.GetRecipesInDashboardAsync(request);
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecipeByIdAsync(long id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);
        return Ok(recipe);
    }

    [HttpPut("{id}/recipe")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRecipeAsync(long id, UpdateRecipeRequest request)
    {
        await _recipeService.UpdateRecipeAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRecipeAsync(long id)
    {
        await _recipeService.DeleteRecipeAsync(id);
        return NoContent();
    }

}
