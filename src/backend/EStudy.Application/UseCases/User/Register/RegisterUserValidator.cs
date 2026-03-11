using EStudy.Application.SharedValidators;
using EStudy.Communication.Requests.Users;
using EStudy.Domain.Extensions;
using FluentValidation;

namespace EStudy.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(user => user.Email).NotEmpty().WithMessage("O email é obrigatório.");
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(user => user.Email.NotEmpty(), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("O email informado é inválido.");
        });
    }
}