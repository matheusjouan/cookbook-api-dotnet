using System.Linq.Expressions;

namespace Cookbook.Core.Interfaces;

public interface IBaseRepository<T>
{
    IQueryable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
