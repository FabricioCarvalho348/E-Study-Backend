using EStudy.Communication.Requests.UserCustomCategories;

namespace EStudy.Application.UseCases.UserCustomCategories.Update;

public interface IUpdateUserCustomCategoryUseCase
{
    Task Execute(long categoryId, RequestUpdateUserCustomCategoryJson request);
}

