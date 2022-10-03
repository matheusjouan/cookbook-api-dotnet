using Cookbook.Application.Request.Auth;
using Cookbook.Application.Response.User;

namespace Cookbook.Application.Services.Interfaces;

public interface IUserService
{
    Task<SignInResponse> CreateUserAsync(CreateUserRequest request);

    Task<SignInResponse> AuthenticateUserAsync(SignInRequest request);

    Task UpdatePasswordAsync(UpdatePasswordUserRequest request);
    Task<UserAuthenticatedResponse> GetUserAuthenticatedInfoAsync();
}
