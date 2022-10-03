using Cookbook.Core.Interfaces;
using Cookbook.Infra.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cookbook.Infra.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly CookbookContext _context;

    public BaseRepository(CookbookContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }
}
