using Cookbook.Application.Request.Auth;
using Cookbook.Core.Excpetions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Cookbook.Application.Validators.Auth;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyName)
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyPhone)
            .Must(ValidPhoneFormat).WithMessage(ResourceErrorMessages.InvalidPhone);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyEmail)
            .EmailAddress().WithMessage(ResourceErrorMessages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ResourceErrorMessages.EmptyPassword)
            .MinimumLength(6).WithMessage(ResourceErrorMessages.InvalidPassword);
    }

    private bool ValidPhoneFormat(string phone)
    {
        var regex = new Regex(@"[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}");
        return regex.IsMatch(phone);
    }
}
