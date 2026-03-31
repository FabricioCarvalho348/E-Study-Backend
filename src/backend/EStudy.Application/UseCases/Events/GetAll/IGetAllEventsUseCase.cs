using EStudy.Communication.Responses.Events;

namespace EStudy.Application.UseCases.Events.GetAll;

public interface IGetAllEventsUseCase
{
    Task<List<ResponseEventJson>> Execute(DateTime startDate, DateTime endDate, string? type);
}

