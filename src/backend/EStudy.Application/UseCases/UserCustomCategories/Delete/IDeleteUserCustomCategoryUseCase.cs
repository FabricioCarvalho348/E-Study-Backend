namespace EStudy.Application.UseCases.UserCustomCategories.Delete;

public interface IDeleteUserCustomCategoryUseCase
{
    Task Execute(long categoryId);
}

