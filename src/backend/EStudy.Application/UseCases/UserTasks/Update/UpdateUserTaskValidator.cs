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
    }
}

