using Bogus;
using Bogus.DataSets;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Response.User;
using Cookbook.Core.Entities;

namespace Cookbook.Tests.Fixture;

public class UserFixture
{
    public CreateUserRequest EntryCreateUserRequest()
    {
        return new CreateUserRequest
        {
            Name = new Name().FullName(),
            Email = "foo@foo.com",
            Phone = "11 9 1111-1111",
            Password = new Internet().Password()
        };
    }

    public CreateUserRequest EntryCreateInvalidUserRequest()
    {
        return new CreateUserRequest
        {
            Name = new Name().FullName(),
            Email = new Random().Next().ToString(),
            Phone = "11 9 1111",
            Password = new Internet().Password()
        };
    }

    public string GenerateString() =>  new Random().Next().ToString();

    public SignInResponse GenerateTokenValid()
    {
        return new SignInResponse
        {
            Token = new Random().Next().ToString(),
            ExpiresIn = DateTime.Now
        };
    }

    public SignInRequest GenerateCredentialsUser()
    {
        return new SignInRequest
        {
            Email = new Internet().Email(),
            Password = new Internet().Password()
        };
    }

    public UpdatePasswordUserRequest GenerateUpdatePasswordRequest()
    {
        return new UpdatePasswordUserRequest
        {
            CurrentPassword = new Internet().Password(),
            NewPassword = new Internet().Password(),
        };
    }

    public UserAuthenticatedResponse GenerateUserAuthenticated()
    {
        return new UserAuthenticatedResponse
        {
            Id = 5,
            Name = new Name().FullName(),
            Email = new Internet().Email()
        };
    }
}


