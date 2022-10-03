using Bogus.DataSets;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Validators.Auth;
using FluentAssertions;

namespace Cookbook.Tests.Validators.Auth;

public class SignInRequestValidatorTest
{
    [Fact]
    public void SignInRequest_WhenPropertiesValid_ReturnsSuccess()
    {
        // Arrange
        var validator = new SignInRequestValidator();

        var request = new SignInRequest
        {
            Email = new Internet().Email(),
            Password = new Internet().Password()
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SignInRequest_WhenSomePropertyInvalid_ReturnsErrors()
    {
        // Arrange
        var validator = new SignInRequestValidator();

        var request = new SignInRequest
        {
            Email = "",
            Password = ""
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(err => err.ErrorMessage.Contains("The email field cannot be empty"))
            .And.Contain(err => err.ErrorMessage.Contains("The password field must be valid, min 6 characteres"));
    }
}
