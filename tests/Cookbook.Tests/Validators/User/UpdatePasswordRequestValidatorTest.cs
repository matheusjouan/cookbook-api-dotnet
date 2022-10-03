using Bogus.DataSets;
using Cookbook.Application.Request.Auth;
using Cookbook.Application.Validators.User;
using FluentAssertions;

namespace Cookbook.Tests.Validators.User;

public class UpdatePasswordRequestValidatorTest
{
    [Fact]
    public void UpdatePasswordRequest_WhenPropertiesValid_ReturnsSuccess()
    {
        // Arrange
        var validator = new UpdatePasswordRequestValidator();
        var request = new UpdatePasswordUserRequest
        {
            CurrentPassword = new Internet().Password(),
            NewPassword = new Internet().Password(),
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void UpdatePasswordRequest_WhenSomePropertyInvalid_ReturnsError()
    {
        // Arrange
        var validator = new UpdatePasswordRequestValidator();
        var request = new UpdatePasswordUserRequest
        {
            CurrentPassword = "",
            NewPassword = new Internet().Password(),
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .NotBeEmpty()
            .And.Contain(err => err.ErrorMessage.Contains("The password cannot be empty"));
    }
}
