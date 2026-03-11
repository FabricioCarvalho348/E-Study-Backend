using EStudy.Communication.Requests.DoLogin;
using EStudy.Communication.Responses.Tokens;
using EStudy.Communication.Responses.Users;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserRepository userRepository,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator accessTokenGenerator)
    : IDoLoginUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encryptedPassword = passwordEncrypter.Encrypt(request.Password);
        
        var user = await userRepository.GetByEmailAndPassword(request.Email, encryptedPassword) ?? throw new InvalidLoginException();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
}