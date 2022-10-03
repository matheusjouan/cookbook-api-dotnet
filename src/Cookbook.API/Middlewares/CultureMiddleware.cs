using System.Globalization;

namespace Cookbook.API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IList<string> _supportLanguage = new List<string>
    {
        "pt-BR",
        "en-US"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        // Criando uma cultura padrão
        var culture = new CultureInfo("pt-BR");

        // Verifica se no Header tem a chave Accept-Language
        if (httpContext.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = httpContext.Request.Headers["Accept-Language"].ToString();

            // Se a lingua conter na nossa lista aplica a utilização
            if(_supportLanguage.Any(x => x.Equals(language)))
                culture = new CultureInfo(language);
        }

        // Obtem os resourcers de acordo com a cultura
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        // Call the next delegate/middleware in the pipeline.
        await _next(httpContext);
    }
}