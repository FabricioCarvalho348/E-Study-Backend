using EStudy.Communication.Responses.UserCustomCategories;

namespace EStudy.Application.UseCases.UserCustomCategories.GetAll;

public interface IGetAllUserCustomCategoriesUseCase
{
    Task<List<ResponseUserCustomCategoryJson>> Execute();
}

