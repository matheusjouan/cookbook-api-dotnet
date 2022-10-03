using AutoMapper;
using Cookbook.Application.Response.Recipes;
using Cookbook.Application.Services;
using Cookbook.Application.Services.Interfaces;
using Cookbook.Core.Entities;
using Cookbook.Core.Excpetions;
using Cookbook.Core.Interfaces;
using Cookbook.Tests.Configuration;
using Cookbook.Tests.Fixture;
using FluentAssertions;
using Moq;
using System.Security.Authentication;

namespace Cookbook.Tests.Services;
public class RecipeServiceTest
{
    private readonly RecipeService _sut;
    private readonly RecipeFixture _recipeFixture;
    private readonly UserFixture _userFixture;
    private readonly Mock<IUserService>_userServiceMock;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly IMapper _mapper;

    public RecipeServiceTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _uow = new Mock<IUnitOfWork>();
        _mapper = AutoMapperConfig.GetConfiguration();
        _userFixture = new UserFixture();
        _recipeFixture = new RecipeFixture();

        _sut = new RecipeService(_uow.Object, _mapper, _userServiceMock.Object);
    }

    [Fact]
    public async Task CreateRecipe_WhenUserIsAuthenticated_ReturnRecipe()
    {
        // Arrange
        var request = _recipeFixture.GenerateRecipeRequest();
        var userAuthenticated = _userFixture.GenerateUserAuthenticated();

        _userServiceMock.Setup(_ => _.GetUserAuthenticatedInfoAsync())
            .ReturnsAsync(userAuthenticated);

        _uow.Setup(_ => _.RecipeRepository.Add(It.IsAny<Recipe>()));

        // Act
        var result = await _sut.CreateRecipeAsync(request);

        // Assert
        _uow.Verify(_ => _.SaveChangeAsync(), Times.Once);
        result.Should().BeEquivalentTo(_mapper.Map<RecipeResponse>(result));
    }

    [Fact]
    public async Task GetRecipe_WhenRecipeExists_ReturnRecipe()
    {
        // Arrange
        var recipe = _recipeFixture.GenerateRecipe();

        _uow.Setup(_ => _.RecipeRepository.GetRecipeById(It.IsAny<long>()))
            .ReturnsAsync(recipe);

        // Act
        var result = await _sut.GetRecipeByIdAsync(recipe.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mapper.Map<RecipeResponse>(result));
    }

    [Fact]
    public async Task GetRecipe_WhenRecipeNotExists_ReturnException()
    {
        // Arrange
        var recipe = _recipeFixture.GenerateRecipe();

        _uow.Setup(_ => _.RecipeRepository.GetRecipeById(It.IsAny<long>()))
            .ReturnsAsync(() => null);

        // Act
        Func<Task<RecipeResponse>> act = async () => await _sut.GetRecipeByIdAsync(recipe.Id);

        // Assert
        await act.Should().ThrowAsync<RecipeNotFoundException>();
    }

    [Fact]
    public async Task DeleteRecipe_WhenRecipeExists_ReturnSuccess()
    {
        // Arrange 
        var recipe = _recipeFixture.GenerateRecipe();
        var user = _userFixture.GenerateUserAuthenticated();

        _userServiceMock.Setup(_ => _.GetUserAuthenticatedInfoAsync())
            .ReturnsAsync(user);

        _uow.Setup(_ => _.RecipeRepository.GetRecipeById(It.IsAny<long>()))
            .ReturnsAsync(recipe);

        // Act
        await _sut.DeleteRecipeAsync(recipe.Id);

        // Assert
        _uow.Verify(_ => _.RecipeRepository.Delete(It.IsAny<Recipe>()), Times.Once);
        _uow.Verify(_ => _.SaveChangeAsync(), Times.Once);    
    }

    [Fact]
    public async Task DeleteRecipe_WhenRecipeNotExists_ReturnException()
    {
        // Arrange 
        var recipe = _recipeFixture.GenerateRecipe();
        var user = _userFixture.GenerateUserAuthenticated();

        _userServiceMock.Setup(_ => _.GetUserAuthenticatedInfoAsync())
            .ReturnsAsync(user);

        _uow.Setup(_ => _.RecipeRepository.GetRecipeById(It.IsAny<long>()))
            .ReturnsAsync(() => null);

        // Act
        Func<Task> act = async() => await _sut.DeleteRecipeAsync(recipe.Id);

        // Assert
        _uow.Verify(_ => _.SaveChangeAsync(), Times.Never);
        await act.Should().ThrowAsync<RecipeNotFoundException>();
    }

    [Fact]
    public async Task UpdateRecipe_WhenRecipeExists_ReturnSuccess()
    {
        // Arrange 
        var request = _recipeFixture.UpdateeRecipeRequest();
        var recipe = _recipeFixture.GenerateRecipe();
        var user = _userFixture.GenerateUserAuthenticated();
        var recipeId = new Random().Next();

        _userServiceMock.Setup(_ => _.GetUserAuthenticatedInfoAsync())
            .ReturnsAsync(user);

        _uow.Setup(_ => _.RecipeRepository.GetRecipeById(It.IsAny<long>()))
            .ReturnsAsync(recipe);

        recipe.Id = user.Id;

        // Act
        await _sut.UpdateRecipeAsync(recipeId, request);

        // Assert
        _uow.Verify(_ => _.RecipeRepository.Update(It.IsAny<Recipe>()), Times.Once);
        _uow.Verify(_ => _.SaveChangeAsync(), Times.Once);
    }

}
