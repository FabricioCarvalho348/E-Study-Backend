using EStudy.Communication.Requests.ChangePassword;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace EStudy.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(
    ILoggedUser loggedUser,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordEncrypter passwordEncrypter)
    : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser1 = await loggedUser.User();

        Validate(request, loggedUser1);
        
        var user = await userRepository.GetById(loggedUser1.Id);
        
        user.Password = passwordEncrypter.Encrypt(request.NewPassword);
        
        userRepository.Update(user);
        
        await unitOfWork.Commit();
    }
    
    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);
        
        var currentPasswordEncrypted = passwordEncrypter.Encrypt(request.Password);
        
        if (currentPasswordEncrypted.Equals(loggedUser.Password).IsFalse())
            result.Errors.Add(new ValidationFailure(string.Empty, "A Senha está incorreta."));
        
        if(result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}