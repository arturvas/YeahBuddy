using FluentValidation;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Exceptions;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourcesMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage("The Email can't be empty");
        RuleFor(user => user.Email).EmailAddress();
        RuleFor(user => user.Password.Length).GreaterThan(5);
    }
}