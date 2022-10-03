using Cookbook.API.Extensions;
using Cookbook.API.Filters;
using Cookbook.Application.Extensions;
using Cookbook.Infra.Extensions;
using Cookbook.Infra.Persistence.Context;
using Cookbook.Infra.Persistence.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Initializer DB
Database.InitializeDb(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers(options =>
    options.Filters.Add(typeof(ValidationFilter)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Acesso aos dados da Request
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
).AddJwtBearer(
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
    }
);

// Inclusão dos Providers de Infra
builder.Services
    .DatabaseMigrations(builder.Configuration)
    .AddCookbookContext(builder.Configuration)
    .AddRepositories();

// Inclusão dos Services
builder.Services
    .AddFlientValidation()
    .AddAutoMapper()
    .AddServicesApplication();

// Todas as URLs do Endpoint serão minúscula no Swagger
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CookbookContext>();

var app = builder.Build();

// Mapeamento do HealtCheck
app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false, 
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK, // OK
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Execução das Migrations
app.MigrationExecution();

// Ativa o Middleware de Exceções
app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Utilizando o middleware
app.UseCultureMiddleware();

app.Run();

public partial class Program { }
