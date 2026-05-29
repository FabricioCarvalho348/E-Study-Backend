using EStudy.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserTaskBuilder
{
    public static UserTask Build(
        string title = "Default Task Title",
        string? description = "Default Task Description",
        DateTime? dueDate = null,
        bool isCompleted = false,
        long userId = 1,
        CategoryEnum? category = null,
        long? customCategoryId = null,
        UserCustomCategory? customCategory = null)
    {
        return new UserTask
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            IsCompleted = isCompleted,
            UserId = userId,
            Category = category,
            CustomCategoryId = customCategoryId,
            CustomCategory = customCategory
        };
    }
}