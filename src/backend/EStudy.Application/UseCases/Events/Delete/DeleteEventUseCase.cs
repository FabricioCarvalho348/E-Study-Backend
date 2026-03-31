using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.Delete;

public class DeleteEventUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IDeleteEventUseCase
{
    public async Task Execute(long eventId)
    {
        var user = await loggedUser.User();
        var eventEntity = await eventRepository.GetById(eventId, user.Id);

        if (eventEntity is null)
            throw new NotFoundException("Evento nao encontrado.");

        eventRepository.Delete(eventEntity);
        await unitOfWork.Commit();
    }
}

