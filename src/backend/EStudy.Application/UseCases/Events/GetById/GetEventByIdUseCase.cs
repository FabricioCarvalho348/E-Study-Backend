using AutoMapper;
using EStudy.Communication.Responses.Events;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.GetById;

public class GetEventByIdUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IMapper mapper) : IGetEventByIdUseCase
{
    public async Task<ResponseEventJson> Execute(long eventId)
    {
        var user = await loggedUser.User();
        var eventEntity = await eventRepository.GetById(eventId, user.Id);

        if (eventEntity is null)
            throw new NotFoundException("Evento nao encontrado.");

        return mapper.Map<ResponseEventJson>(eventEntity);
    }
}

