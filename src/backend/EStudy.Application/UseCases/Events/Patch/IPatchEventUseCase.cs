using EStudy.Communication.Requests.Events;

namespace EStudy.Application.UseCases.Events.Patch;

public interface IPatchEventUseCase
{
    Task Execute(long eventId, RequestPatchEventJson request);
}

