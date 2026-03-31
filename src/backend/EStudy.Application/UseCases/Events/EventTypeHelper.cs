namespace EStudy.Application.UseCases.Events;

public static class EventTypeHelper
{
    public static string Sanitize(string? type) => string.IsNullOrWhiteSpace(type) ? string.Empty : type.Trim();
}



