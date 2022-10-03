using Cookbook.Core.Entities;

namespace Cookbook.Core.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserByEmailAsync(string email);
    Task<User> AuthenticationUserAsync(string email, string password);
}
