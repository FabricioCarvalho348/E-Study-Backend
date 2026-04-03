namespace EStudy.Exception.ExceptionsBase;

public static class AppErrorCodes
{
    public static class General
    {
        public const string Unexpected = "GEN-001";
        public const string NotFound = "GEN-404";
        public const string Unauthorized = "GEN-401";
        public const string Validation = "GEN-400";
    }

    public static class Auth
    {
        public const string InvalidCredentials = "AUTH-001";
        public const string RefreshTokenNotFound = "AUTH-002";
        public const string RefreshTokenExpired = "AUTH-003";
        public const string AuthorizationHeaderMissing = "AUTH-004";
        public const string UserNotFound = "AUTH-005";
        public const string TokenExpired = "AUTH-006";
    }
}

