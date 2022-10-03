using Cookbook.Core.Entities;
using Cookbook.Core.Interfaces;
using Cookbook.Infra.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infra.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly CookbookContext _context;

    public UserRepository(CookbookContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AuthenticationUserAsync(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

}
