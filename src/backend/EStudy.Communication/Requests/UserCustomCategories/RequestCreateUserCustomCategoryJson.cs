using System.Text.Json.Serialization;

namespace EStudy.Communication.Requests.UserCustomCategories;

[JsonConverter(typeof(RequestUserCustomCategoryJsonConverter))]
public class RequestCreateUserCustomCategoryJson
{
    public string Name { get; set; } = string.Empty;
}

