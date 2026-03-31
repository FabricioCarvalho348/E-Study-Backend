namespace EStudy.Application.UseCases.Events.Delete;

public interface IDeleteEventUseCase
{
    Task Execute(long eventId);
}

