using AutoMapper;
using EStudy.Application.Common.ErrorHandling;
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
    IUserWriteOnlyRepository writeOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
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

        if (user.UserIdentifier == Guid.Empty)
            user.UserIdentifier = Guid.NewGuid();

        await writeOnlyRepository.Add(user);

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

        var emailExist = await readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "Ja existe um usuario cadastrado com este email")
            {
                ErrorCode = AppErrorCodes.General.Validation,
                PropertyName = "email"
            });

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}