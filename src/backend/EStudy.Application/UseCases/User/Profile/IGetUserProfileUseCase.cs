using EStudy.Communication.Responses.Users;

namespace EStudy.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}