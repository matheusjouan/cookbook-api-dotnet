using Cookbook.Core.Interfaces;
using Cookbook.Infra.Persistence.Context;
using Cookbook.Infra.Persistence.Migrations;
using Cookbook.Infra.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Cookbook.Infra.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection DatabaseMigrations(this IServiceCollection services, IConfiguration config)
    {
        var fullConnectionString = config.GetConnectionString("FullConnectionString");

        services.AddFluentMigratorCore()
            .ConfigureRunner(config => config
                .AddMySql5()
                .WithGlobalConnectionString(fullConnectionString)
                .ScanIn(typeof(CookbookMigrations_v1).Assembly).For.Migrations()
            )
            .AddLogging(c => c.AddFluentMigratorConsole());

        return services;
    }


    public static IApplicationBuilder MigrationExecution(this IApplicationBuilder app)
    {
        // Cria um escopo para utilizar um Serviço sem ser injetado por Injeção de dependência
        // Como a classe é estática, não conseguimos passar um serviço através do construtor
        // por isso criamos um escopo para utilizar o serviço
        using var scope = app.ApplicationServices.CreateScope();
        
        // Pega o serviço do IMigrationRunner
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        // Lista todas as migrations
        runner.ListMigrations();

        // Executa o método Up() das Migrations
        runner.MigrateUp();

        return app;
    }

    public static IServiceCollection AddCookbookContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("FullConnectionString");

        services.AddDbContext<CookbookContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();

        return services;
    }

}
