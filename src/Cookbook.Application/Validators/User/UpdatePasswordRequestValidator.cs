using Cookbook.Application.Request.Auth;
using Cookbook.Core.Excpetions;
using FluentValidation;

namespace Cookbook.Application.Validators.User;

public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordUserRequest>
{
	public UpdatePasswordRequestValidator()
	{
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyPassword)
            .MinimumLength(6).WithMessage(ResourceErrorMessages.InvalidPassword);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyPassword)
            .MinimumLength(6).WithMessage(ResourceErrorMessages.InvalidPassword);
    }
}
