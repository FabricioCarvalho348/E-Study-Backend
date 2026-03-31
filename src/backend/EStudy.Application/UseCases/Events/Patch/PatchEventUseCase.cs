using EStudy.Communication.Requests.Events;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.Patch;

public class PatchEventUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IPatchEventUseCase
{
    public async Task Execute(long eventId, RequestPatchEventJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var eventEntity = await eventRepository.GetById(eventId, user.Id);

        if (eventEntity is null)
            throw new NotFoundException("Evento nao encontrado.");

        if (request.Title is not null)
            eventEntity.Title = request.Title;

        if (request.Description is not null)
            eventEntity.Description = request.Description;

        if (request.Type is not null)
            eventEntity.Type = EventTypeHelper.Sanitize(request.Type);

        var startDateTime = request.StartDateTime ?? eventEntity.StartDateTime;
        var endDateTime = request.EndDateTime ?? eventEntity.EndDateTime;

        if (startDateTime > endDateTime)
            throw new ErrorOnValidationException(["A data/hora de inicio deve ser menor ou igual a data/hora de termino."]);

        eventEntity.StartDateTime = startDateTime;
        eventEntity.EndDateTime = endDateTime;

        if (request.IsAllDay is not null)
            eventEntity.IsAllDay = request.IsAllDay.Value;

        eventRepository.Update(eventEntity);
        await unitOfWork.Commit();
    }

    private static async Task Validate(RequestPatchEventJson request)
    {
        var validator = new PatchEventValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}


