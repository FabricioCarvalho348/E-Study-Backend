using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using EStudy.Communication.Requests.ChangePassword;
using EStudy.Communication.Requests.DoLogin;
using FluentAssertions;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordTest : EStudyClassFixture
{
    private const string Method = "user/change-password";

    private readonly string _password;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(Method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };

        response = await DoPost(method: "login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost(method: "login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("pt-BR")]
    public async Task Error_NewPassword_Empty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            Password = _password,
            NewPassword = string.Empty
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(Method, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var details = responseData.RootElement.GetProperty("details").EnumerateArray();
        details.Should().ContainSingle();
        details.First().GetProperty("code").GetString().Should().Be("GEN-400");

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.Should().ContainSingle();
    }
}