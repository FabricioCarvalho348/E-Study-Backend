using EStudy.Communication.Requests.Events;
using FluentValidation;

namespace EStudy.Application.UseCases.Events.Create;

public class CreateEventValidator : AbstractValidator<RequestCreateEventJson>
{
    public CreateEventValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("O titulo do evento e obrigatorio.")
            .MaximumLength(120).WithMessage("O titulo do evento deve ter no maximo 120 caracteres.");

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("A descricao do evento deve ter no maximo 500 caracteres.")
            .When(request => string.IsNullOrWhiteSpace(request.Description) == false);

        RuleFor(request => request.Type)
            .MaximumLength(80).WithMessage("O tipo do evento deve ter no maximo 80 caracteres.")
            .When(request => string.IsNullOrWhiteSpace(request.Type) == false);

        RuleFor(request => request)
            .Must(request => request.StartDateTime <= request.EndDateTime)
            .WithMessage("A data/hora de inicio deve ser menor ou igual a data/hora de termino.");
    }
}



