using Cookbook.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infra.Persistence.Context;

public class CookbookContext : DbContext
{
	public CookbookContext(DbContextOptions<CookbookContext> options) : base (options) { }

	public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }

    protected override void  OnModelCreating(ModelBuilder mb)
	{
		// Aplica toda as configurações que consta forem definido onde esta armazenado esse assembly
		mb.ApplyConfigurationsFromAssembly(typeof(CookbookContext).Assembly);
	}
}
