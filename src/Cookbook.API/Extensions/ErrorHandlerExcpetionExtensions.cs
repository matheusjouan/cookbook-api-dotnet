using Cookbook.Core.Excpetions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace Cookbook.API.Extensions;

public static class ErrorHandlerExcpetionExtensions
{
    public static void UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (contextFeature != null)
                {
                    var error = GetError(contextFeature.Error);

                    context.Response.StatusCode = error.Status;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                }
            });
        });
    }

    private static Error GetError(Exception ex)
    {
        var excpetion = ex as ExceptionsBase;

        if (excpetion?.Error != null)
            return excpetion.Error;

        return new Error
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ex.Data["Title"]?.ToString() ?? ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
            Detail = ex.Message
        };
    }
}
