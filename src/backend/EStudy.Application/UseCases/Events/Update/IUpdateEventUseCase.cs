using EStudy.Communication.Requests.Events;

namespace EStudy.Application.UseCases.Events.Update;

public interface IUpdateEventUseCase
{
    Task Execute(long eventId, RequestUpdateEventJson request);
}

