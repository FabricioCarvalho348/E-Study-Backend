using EStudy.Domain.Entities;

namespace EStudy.Communication.Responses.UserCustomCategories;

public class ResponseCategoryEnumJson
{
    public int Value { get; set; }
    public string Name { get; set; } = string.Empty;

    public static List<ResponseCategoryEnumJson> GetAll()
    {
        return Enum
            .GetValues<CategoryEnum>()
            .Select(category => new ResponseCategoryEnumJson
            {
                Value = (int)category,
                Name = category.ToString()
            })
            .ToList();
    }
}


