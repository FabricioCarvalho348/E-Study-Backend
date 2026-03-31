using EStudy.Communication.Responses.Events;

namespace EStudy.Application.UseCases.Events.GetById;

public interface IGetEventByIdUseCase
{
    Task<ResponseEventJson> Execute(long eventId);
}

