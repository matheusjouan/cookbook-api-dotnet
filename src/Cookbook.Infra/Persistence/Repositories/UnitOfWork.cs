using Cookbook.Core.Interfaces;
using Cookbook.Infra.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cookbook.Infra.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly CookbookContext _context;
    private bool _disposed;

    private IDbContextTransaction _transaction;

    public IUserRepository UserRepository { get; }

    public IRecipeRepository RecipeRepository { get; }


    public UnitOfWork(CookbookContext context, IUserRepository userRepository, 
        IRecipeRepository recipeRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RecipeRepository = recipeRepository;
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await _transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();

        _disposed = true;
    }
}
