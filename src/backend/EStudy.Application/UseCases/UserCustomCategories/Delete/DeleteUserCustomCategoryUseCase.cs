using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserCustomCategories.Delete;

public class DeleteUserCustomCategoryUseCase(
    ILoggedUser loggedUser,
    IUserCustomCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IDeleteUserCustomCategoryUseCase
{
    public async Task Execute(long categoryId)
    {
        var user = await loggedUser.User();
        var category = await categoryRepository.GetById(categoryId, user.Id);

        if (category is null)
            throw new NotFoundException("Categoria nao encontrada.");

        categoryRepository.Delete(category);

        await unitOfWork.Commit();
    }
}

