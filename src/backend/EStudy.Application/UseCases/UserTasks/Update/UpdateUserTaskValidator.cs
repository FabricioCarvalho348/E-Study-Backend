using EStudy.Domain.Entities;
using EStudy.Communication.Requests.UserTasks;
using FluentValidation;

namespace EStudy.Application.UseCases.UserTasks.Update;

public class UpdateUserTaskValidator : AbstractValidator<RequestUpdateUserTaskJson>
{
    public UpdateUserTaskValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("O titulo da tarefa e obrigatorio.")
            .MaximumLength(120).WithMessage("O titulo da tarefa deve ter no maximo 120 caracteres.");

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("A descricao da tarefa deve ter no maximo 500 caracteres.")
            .When(request => string.IsNullOrWhiteSpace(request.Description) == false);

        RuleFor(request => request.Category)
            .Must(category => category is null || Enum.IsDefined(typeof(CategoryEnum), category))
            .WithMessage("A categoria fixa informada e invalida.");

        RuleFor(request => request.CustomCategoryId)
            .Must(customCategoryId => customCategoryId is null || customCategoryId > 0)
            .WithMessage("O identificador da categoria deve ser maior que zero.")
            .When(request => request.Category is null && request.CustomCategoryId.HasValue);

        RuleFor(request => request)
            .Must(request =>
            {
                var hasPredefinedCategory = request.Category.HasValue;
                var hasCustomCategory = request.CustomCategoryId.HasValue && request.CustomCategoryId > 0;
                // Allow: no category, predefined only, or custom only. Disallow: both simultaneously
                return !(hasPredefinedCategory && hasCustomCategory);
            })
            .WithMessage("Nao e permitido selecionar uma categoria predefinida e uma categoria personalizada simultaneamente.");
    }
}

