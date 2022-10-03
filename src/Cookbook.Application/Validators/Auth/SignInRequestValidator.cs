using Cookbook.Application.Request.Auth;
using Cookbook.Core.Excpetions;
using FluentValidation;

namespace Cookbook.Application.Validators.Auth;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
	public SignInRequestValidator()
	{
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyEmail)
            .EmailAddress().WithMessage(ResourceErrorMessages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyPassword)
            .MinimumLength(6).WithMessage(ResourceErrorMessages.InvalidPassword);
    }
}
