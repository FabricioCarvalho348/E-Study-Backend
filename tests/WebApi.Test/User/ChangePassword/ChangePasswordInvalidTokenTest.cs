using System.Net;
using CommonTestUtilities.Tokens;
using EStudy.Communication.Requests.ChangePassword;
using FluentAssertions;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory webApplication)
    : EStudyClassFixture(webApplication)
{
    private const string Method = "user/change-password";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(Method, request, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(Method, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var request = new RequestChangePasswordJson();

        var response = await DoPut(Method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}