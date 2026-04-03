using AutoMapper;
using EStudy.Communication.Responses.Events;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.GetAll;

public class GetAllEventsUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IMapper mapper) : IGetAllEventsUseCase
{
    public async Task<List<ResponseEventJson>> Execute(DateTime startDate, DateTime endDate, string? type)
    {
        if (startDate > endDate)
            throw new ErrorOnValidationException(
            [
                new AppError(
                    AppErrorCodes.General.Validation,
                    "A data inicial deve ser menor ou igual a data final.")
            ]);

        string? sanitizedType = null;
        if (string.IsNullOrWhiteSpace(type) == false)
            sanitizedType = EventTypeHelper.Sanitize(type);

        var user = await loggedUser.User();
        var events = await eventRepository.GetByDateRange(user.Id, startDate, endDate, sanitizedType);

        return mapper.Map<List<ResponseEventJson>>(events);
    }
}
