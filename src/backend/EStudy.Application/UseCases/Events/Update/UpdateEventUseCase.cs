using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.Events;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.Update;

public class UpdateEventUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IUpdateEventUseCase
{
    public async Task Execute(long eventId, RequestUpdateEventJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var eventEntity = await eventRepository.GetById(eventId, user.Id);

        if (eventEntity is null)
            throw new NotFoundException("Evento nao encontrado.");

        eventEntity.Title = request.Title;
        eventEntity.Description = request.Description;
        eventEntity.Type = EventTypeHelper.Sanitize(request.Type);
        eventEntity.StartDateTime = request.StartDateTime;
        eventEntity.EndDateTime = request.EndDateTime;
        eventEntity.IsAllDay = request.IsAllDay;

        eventRepository.Update(eventEntity);
        await unitOfWork.Commit();
    }

    private static async Task Validate(RequestUpdateEventJson request)
    {
        var validator = new UpdateEventValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}
