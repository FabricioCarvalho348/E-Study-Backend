using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserCustomCategories;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserCustomCategories.Update;

public class UpdateUserCustomCategoryUseCase(
    ILoggedUser loggedUser,
    IUserCustomCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IUpdateUserCustomCategoryUseCase
{
    public async Task Execute(long categoryId, RequestUpdateUserCustomCategoryJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var category = await categoryRepository.GetById(categoryId, user.Id);

        if (category is null)
            throw new NotFoundException("Categoria nao encontrada.");

        category.Name = request.Name;

        categoryRepository.Update(category);
        await unitOfWork.Commit();
    }

    private static async Task Validate(RequestUpdateUserCustomCategoryJson request)
    {
        var validator = new UpdateUserCustomCategoryValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}

