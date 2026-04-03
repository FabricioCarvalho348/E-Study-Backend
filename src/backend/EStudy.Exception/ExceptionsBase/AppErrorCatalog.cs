namespace EStudy.Exception.ExceptionsBase;

public static class AppErrorCatalog
{
    private static readonly Dictionary<string, Dictionary<string, string>> Messages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["pt-BR"] = new(StringComparer.OrdinalIgnoreCase)
        {
            [AppErrorCodes.General.Unexpected] = "Ocorreu um erro inesperado.",
            [AppErrorCodes.General.NotFound] = "Recurso nao encontrado.",
            [AppErrorCodes.General.Unauthorized] = "Nao autorizado.",
            [AppErrorCodes.General.Validation] = "Existem erros de validacao.",
            [AppErrorCodes.Auth.InvalidCredentials] = "Email ou senha invalidos.",
            [AppErrorCodes.Auth.RefreshTokenNotFound] = "Refresh token nao encontrado.",
            [AppErrorCodes.Auth.RefreshTokenExpired] = "Refresh token expirado.",
            [AppErrorCodes.Auth.AuthorizationHeaderMissing] = "Cabecalho de autorizacao nao informado.",
            [AppErrorCodes.Auth.UserNotFound] = "Usuario nao encontrado.",
            [AppErrorCodes.Auth.TokenExpired] = "Token expirado."
        },
        ["en-US"] = new(StringComparer.OrdinalIgnoreCase)
        {
            [AppErrorCodes.General.Unexpected] = "An unexpected error occurred.",
            [AppErrorCodes.General.NotFound] = "Resource not found.",
            [AppErrorCodes.General.Unauthorized] = "Unauthorized.",
            [AppErrorCodes.General.Validation] = "There are validation errors.",
            [AppErrorCodes.Auth.InvalidCredentials] = "Invalid email or password.",
            [AppErrorCodes.Auth.RefreshTokenNotFound] = "Refresh token not found.",
            [AppErrorCodes.Auth.RefreshTokenExpired] = "Refresh token has expired.",
            [AppErrorCodes.Auth.AuthorizationHeaderMissing] = "Authorization header is missing.",
            [AppErrorCodes.Auth.UserNotFound] = "User not found.",
            [AppErrorCodes.Auth.TokenExpired] = "Token has expired."
        }
    };

    public static string Resolve(string code, string? acceptLanguage, string fallbackMessage)
    {
        if (string.IsNullOrWhiteSpace(code))
            return fallbackMessage;

        var culture = NormalizeCulture(acceptLanguage);
        if (culture is not null && Messages.TryGetValue(culture, out var cultureMessages) && cultureMessages.TryGetValue(code, out var localizedMessage))
            return localizedMessage;

        if (Messages["pt-BR"].TryGetValue(code, out var defaultMessage))
            return defaultMessage;

        return fallbackMessage;
    }

    public static string GetDefaultMessage(string code)
    {
        if (Messages["pt-BR"].TryGetValue(code, out var message))
            return message;

        return Messages["pt-BR"][AppErrorCodes.General.Unexpected];
    }

    private static string? NormalizeCulture(string? acceptLanguage)
    {
        if (string.IsNullOrWhiteSpace(acceptLanguage))
            return "pt-BR";

        var firstCulture = acceptLanguage.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(value => value.Trim().Split(';')[0])
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(firstCulture))
            return "pt-BR";

        if (Messages.ContainsKey(firstCulture))
            return firstCulture;

        if (firstCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase))
            return "en-US";

        if (firstCulture.StartsWith("pt", StringComparison.OrdinalIgnoreCase))
            return "pt-BR";

        return "pt-BR";
    }
}

