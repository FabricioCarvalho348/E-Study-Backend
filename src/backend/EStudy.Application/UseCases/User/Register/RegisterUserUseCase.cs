using AutoMapper;
using EStudy.Communication.Requests.Users;
using EStudy.Communication.Responses.Tokens;
using EStudy.Communication.Responses.Users;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserRepository userRepository,
    IMapper mapper,
    IPasswordEncrypter passwordEncrypter,
    IUnitOfWork unitOfWork,
    IAccessTokenGenerator accessTokenGenerator)
    : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = mapper.Map<Domain.Entities.User>(request);
        user.Password = passwordEncrypter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        
        await userRepository.Add(user);
        
        await unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        
        var result = await validator.ValidateAsync(request);

        var emailExist = await userRepository.ExistActiveUserWithEmail(request.Email);

        if(emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "Já existe um usuário cadastrado com este email."));
        }
        
        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}