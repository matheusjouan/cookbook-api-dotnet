using Cookbook.Application.Response.User;

namespace Cookbook.Application.Services.Interfaces;

public interface ITokenService
{
    SignInResponse GenerateToken(string email);
}
