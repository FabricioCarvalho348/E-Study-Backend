using AutoMapper;
using EStudy.Communication.Responses.Users;
using EStudy.Domain.Services.LoggedUser;

namespace EStudy.Application.UseCases.User.Profile;

public class GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper) : IGetUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await loggedUser.User();
        
        return mapper.Map<ResponseUserProfileJson>(user);
    }
}