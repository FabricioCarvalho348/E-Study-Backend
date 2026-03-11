using EStudy.Communication.Requests.Users;
using EStudy.Domain.Extensions;
using FluentValidation;

namespace EStudy.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(request => request.Email).NotEmpty().WithMessage("O email é obrigatório.");
        
        When(request => request.Email.NotEmpty(), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage("O email deve ser válido.");
        });
    }
}