using System.Text.Json;
using System.Text.Json.Serialization;

namespace EStudy.Communication.Requests.UserCustomCategories;

public sealed class RequestUpdateUserCustomCategoryJsonConverter : JsonConverter<RequestUpdateUserCustomCategoryJson>
{
    public override RequestUpdateUserCustomCategoryJson Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return new RequestUpdateUserCustomCategoryJson { Name = reader.GetString() ?? string.Empty };

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected a string or object for custom category request.");

        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var name = GetString(root, "name", "Name")
            ?? GetString(root, "category", "Category")
            ?? GetString(root, "title", "Title")
            ?? string.Empty;

        return new RequestUpdateUserCustomCategoryJson { Name = name };
    }

    public override void Write(Utf8JsonWriter writer, RequestUpdateUserCustomCategoryJson value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteEndObject();
    }

    private static string? GetString(JsonElement root, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!root.TryGetProperty(propertyName, out var property))
                continue;

            if (property.ValueKind == JsonValueKind.String)
                return property.GetString();
        }

        return null;
    }
}
