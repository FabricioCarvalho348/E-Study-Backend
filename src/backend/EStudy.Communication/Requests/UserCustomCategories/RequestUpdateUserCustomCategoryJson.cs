using System.Text.Json.Serialization;

namespace EStudy.Communication.Requests.UserCustomCategories;

[JsonConverter(typeof(RequestUpdateUserCustomCategoryJsonConverter))]
public class RequestUpdateUserCustomCategoryJson
{
    public string Name { get; set; } = string.Empty;
}

