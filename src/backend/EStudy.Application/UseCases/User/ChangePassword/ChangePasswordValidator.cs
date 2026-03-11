using EStudy.Application.SharedValidators;
using EStudy.Communication.Requests.ChangePassword;
using FluentValidation;

namespace EStudy.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}