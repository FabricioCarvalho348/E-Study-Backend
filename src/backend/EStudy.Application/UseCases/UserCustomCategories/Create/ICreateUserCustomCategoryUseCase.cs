using EStudy.Communication.Requests.UserCustomCategories;
using EStudy.Communication.Responses.UserCustomCategories;

namespace EStudy.Application.UseCases.UserCustomCategories.Create;

public interface ICreateUserCustomCategoryUseCase
{
    Task<ResponseUserCustomCategoryJson> Execute(RequestCreateUserCustomCategoryJson request);
}

