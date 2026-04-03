using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.Users;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.User.Update;

public class UpdateUserUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork)
    : IUpdateUserUseCase
{
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser1 = await loggedUser.User();

        await Validate(request, loggedUser1.Email);

        var user = await repository.GetById(loggedUser1.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = await validator.ValidateAsync(request);

        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var userExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", "Ja existe um usuario com este email")
                {
                    ErrorCode = AppErrorCodes.General.Validation
                });
        }

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}