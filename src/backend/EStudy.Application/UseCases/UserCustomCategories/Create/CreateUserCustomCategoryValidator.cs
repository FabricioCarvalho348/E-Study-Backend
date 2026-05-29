using EStudy.Communication.Requests.UserCustomCategories;
using FluentValidation;

namespace EStudy.Application.UseCases.UserCustomCategories.Create;

public class CreateUserCustomCategoryValidator : AbstractValidator<RequestCreateUserCustomCategoryJson>
{
    public CreateUserCustomCategoryValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("O nome da categoria e obrigatorio.")
            .MaximumLength(100).WithMessage("O nome da categoria deve ter no maximo 100 caracteres.");
    }
}

