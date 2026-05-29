using AutoMapper;
using EStudy.Communication.Responses.UserCustomCategories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Services.LoggedUser;

namespace EStudy.Application.UseCases.UserCustomCategories.GetAll;

public class GetAllUserCustomCategoriesUseCase(
    ILoggedUser loggedUser,
    IUserCustomCategoryRepository categoryRepository,
    IMapper mapper) : IGetAllUserCustomCategoriesUseCase
{
    public async Task<List<ResponseUserCustomCategoryJson>> Execute()
    {
        var user = await loggedUser.User();
        var categories = await categoryRepository.GetAllByUserId(user.Id);

        return mapper.Map<List<ResponseUserCustomCategoryJson>>(categories);
    }
}

