using EStudy.Communication.Requests.Events;
using EStudy.Communication.Responses.Events;

namespace EStudy.Application.UseCases.Events.Create;

public interface ICreateEventUseCase
{
    Task<ResponseEventJson> Execute(RequestCreateEventJson request);
}

