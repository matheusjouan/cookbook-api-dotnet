
using Cookbook.Application.Response.User;
using Cookbook.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cookbook.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public SignInResponse GenerateToken(string email)
    {
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var key = _config["Jwt:Key"];
        var expiresIn = DateTime.Now.AddHours(double.Parse(_config["Jwt:ExpiresInHours"]));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("email", email),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: expiresIn,
            claims: claims,
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);

        return new SignInResponse
        {
            Token = stringToken,
            ExpiresIn = expiresIn
        };
    }
}
