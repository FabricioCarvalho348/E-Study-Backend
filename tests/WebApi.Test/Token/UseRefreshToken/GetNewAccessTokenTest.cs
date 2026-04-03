using System.Net;
using System.Text.Json;
using EStudy.Communication.Requests.Tokens;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Token.UseRefreshToken;

public class GetNewAccessTokenTest : EStudyClassFixture
{
    private const string Method = "token";

    private readonly string _userRefreshToken;

    public GetNewAccessTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userRefreshToken = factory.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = _userRefreshToken
        };

        var response = await DoPost($"{Method}/refresh-token", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("en", "AUTH-002", "Refresh token not found.")]
    [InlineData("pt-BR", "AUTH-002", "Refresh token nao encontrado.")]
    public async Task Error_Login_Invalid(string culture, string expectedCode, string expectedMessage)
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = "InvalidRefreshToken"
        };

        var response = await DoPost($"{Method}/refresh-token", request, culture: culture);

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