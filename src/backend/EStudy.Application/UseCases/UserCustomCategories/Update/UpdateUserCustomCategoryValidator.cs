using EStudy.Communication.Requests.UserCustomCategories;
using FluentValidation;

namespace EStudy.Application.UseCases.UserCustomCategories.Update;

public class UpdateUserCustomCategoryValidator : AbstractValidator<RequestUpdateUserCustomCategoryJson>
{
    public UpdateUserCustomCategoryValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("O nome da categoria e obrigatorio.")
            .MaximumLength(100).WithMessage("O nome da categoria deve ter no maximo 100 caracteres.");
    }
}

