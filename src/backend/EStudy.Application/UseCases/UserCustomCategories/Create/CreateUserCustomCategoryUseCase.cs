using AutoMapper;
using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserCustomCategories;
using EStudy.Communication.Responses.UserCustomCategories;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserCustomCategories.Create;

public class CreateUserCustomCategoryUseCase(
    ILoggedUser loggedUser,
    IUserCustomCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICreateUserCustomCategoryUseCase
{
    public async Task<ResponseUserCustomCategoryJson> Execute(RequestCreateUserCustomCategoryJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();

        var category = mapper.Map<Domain.Entities.UserCustomCategory>(request);
        category.UserId = user.Id;

        await categoryRepository.Add(category);
        await unitOfWork.Commit();

        return mapper.Map<ResponseUserCustomCategoryJson>(category);
    }

    private static async Task Validate(RequestCreateUserCustomCategoryJson request)
    {
        var validator = new CreateUserCustomCategoryValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}

