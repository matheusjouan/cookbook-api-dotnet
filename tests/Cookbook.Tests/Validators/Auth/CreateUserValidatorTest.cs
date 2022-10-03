using Cookbook.Application.Request.Auth;
using Cookbook.Application.Validators.Auth;
using FluentAssertions;
using Xunit;

namespace Cookbook.Tests.Validators.Auth;

public class CreateUserValidatorTest
{
    [Fact]
    public void CreateUser_WhenSomePropertyInvalid_ReturnErrors()
    {
        var validator = new CreateUserValidator();

        var request = new CreateUserRequest
        {
            Email = "foo@foo.com",
            Name = "",
            Password = "123456",
            Phone = ""
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should()
            .NotBeEmpty()
            .And.Contain(err => err.ErrorMessage.Contains("The name field cannot be empty"))
            .And.Contain(err => err.ErrorMessage.Contains("The phone field cannot be empty"));

        //Assert.False(result.IsValid);
        //Assert.True(result.Errors.Any());
    }

    [Fact]
    public void CreateUser_WhenProperyValid_ReturnSuccess()
    {
        var validator = new CreateUserValidator();

        var request = new CreateUserRequest
        {
            Email = "foo@foo.com",
            Name = "foo",
            Password = "123456",
            Phone = "11 1 1111-1111"
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
