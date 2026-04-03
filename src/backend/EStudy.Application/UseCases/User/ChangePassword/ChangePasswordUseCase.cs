using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.ChangePassword;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordEncrypter passwordEncrypter,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordEncrypter.IsValid(request.Password, loggedUser.Password).IsFalse())
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "Senha atual incorreta")
            {
                ErrorCode = AppErrorCodes.General.Validation,
                PropertyName = "password"
            });

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}