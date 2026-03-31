using EStudy.Communication.Requests.Events;
using FluentValidation;

namespace EStudy.Application.UseCases.Events.Patch;

public class PatchEventValidator : AbstractValidator<RequestPatchEventJson>
{
    public PatchEventValidator()
    {
        RuleFor(request => request.Title)
            .MaximumLength(120).WithMessage("O titulo do evento deve ter no maximo 120 caracteres.")
            .When(request => request.Title is not null);

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("A descricao do evento deve ter no maximo 500 caracteres.")
            .When(request => request.Description is not null);

        RuleFor(request => request.Type)
            .Must(type => type is null || string.IsNullOrWhiteSpace(type) == false)
            .WithMessage("Informe o tipo do evento.")
            .MaximumLength(80).WithMessage("O tipo do evento deve ter no maximo 80 caracteres.")
            .When(request => request.Type is not null);

        RuleFor(request => request)
            .Must(request => request.Title is not null
                             || request.Description is not null
                             || request.Type is not null
                             || request.StartDateTime is not null
                             || request.EndDateTime is not null
                             || request.IsAllDay is not null)
            .WithMessage("Informe ao menos um campo para atualizar o evento.");
    }
}


