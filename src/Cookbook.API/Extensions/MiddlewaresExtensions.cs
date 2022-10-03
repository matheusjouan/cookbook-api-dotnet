using Cookbook.API.Middlewares;

namespace Cookbook.API.Extensions;

public static class MiddlewaresExtensions
{
    public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CultureMiddleware>();
    }
}
