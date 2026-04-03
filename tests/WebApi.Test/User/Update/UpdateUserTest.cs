using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Update;

public class UpdateUserTest : EStudyClassFixture
{
    private const string Method = "user/update-profile";

    private readonly Guid _userIdentifier;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(Method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("pt-BR")]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

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