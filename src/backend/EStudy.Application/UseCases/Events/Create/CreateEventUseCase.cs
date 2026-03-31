using AutoMapper;
using EStudy.Communication.Requests.Events;
using EStudy.Communication.Responses.Events;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Events.Create;

public class CreateEventUseCase(
    ILoggedUser loggedUser,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICreateEventUseCase
{
    public async Task<ResponseEventJson> Execute(RequestCreateEventJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();

        var eventEntity = mapper.Map<Domain.Entities.Event>(request);
        eventEntity.Type = EventTypeHelper.Sanitize(request.Type);
        eventEntity.UserId = user.Id;

        await eventRepository.Add(eventEntity);
        await unitOfWork.Commit();

        return mapper.Map<ResponseEventJson>(eventEntity);
    }

    private static async Task Validate(RequestCreateEventJson request)
    {
        var validator = new CreateEventValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}


