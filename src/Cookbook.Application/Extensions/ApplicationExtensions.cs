using AutoMapper;
using Cookbook.Application.Mapping;
using Cookbook.Application.Services;
using Cookbook.Application.Services.Interfaces;
using Cookbook.Application.Validators.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddFlientValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        services.AddSingleton(mapper);

        return services;
    }

    public static IServiceCollection AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserServices>();
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRecipeService, RecipeService>();

        return services;
    }
}
