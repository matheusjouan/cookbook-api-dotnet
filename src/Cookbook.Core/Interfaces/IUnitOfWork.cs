namespace Cookbook.Core.Interfaces;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public IRecipeRepository RecipeRepository { get; }

    Task SaveChangeAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    void Dispose();
}
