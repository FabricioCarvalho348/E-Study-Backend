using AutoMapper;
using EStudy.Communication.Requests.Users;
using EStudy.Communication.Responses.Tokens;
using EStudy.Communication.Responses.Users;
using EStudy.Domain.Entities;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Token;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator accessTokenGenerator,
    IMapper mapper,
    ITokenRepository tokenRepository,
    IRefreshTokenGenerator refreshTokenGenerator)
    : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = mapper.Map<Domain.Entities.User>(request);
        user.Password = passwordEncrypter.Encrypt(request.Password);

        await userRepository.Add(user);

        await unitOfWork.Commit();

        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = refreshTokenGenerator.Generate();

        await tokenRepository.SaveNewRefreshToken(new RefreshToken
        {
            Value = refreshToken,
            UserId = user.Id
        });

        await unitOfWork.Commit();

        return refreshToken;
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = await validator.ValidateAsync(request);

        var emailExist = await userRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "Já existe um usuário cadastrado com este email."));

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}