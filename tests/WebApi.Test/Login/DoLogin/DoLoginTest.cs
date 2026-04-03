using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using EStudy.Communication.Requests.DoLogin;
using Xunit;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : EStudyClassFixture
{
    private readonly string _method = "login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(method: _method, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("en", "AUTH-001", "Invalid email or password.")]
    [InlineData("pt-BR", "AUTH-001", "Email ou senha invalidos.")]
    public async Task Error_Login_Invalid(string culture, string expectedCode, string expectedMessage)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(method: _method, request: request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

        var details = responseData.RootElement.GetProperty("details").EnumerateArray();
        details.Should().ContainSingle();
        details.First().GetProperty("code").GetString().Should().Be(expectedCode);
        details.First().GetProperty("message").GetString().Should().Be(expectedMessage);
    }
}