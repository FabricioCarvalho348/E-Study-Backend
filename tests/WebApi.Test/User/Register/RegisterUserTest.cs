using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : EStudyClassFixture(factory)
{
    private readonly string _method = "user/register";

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(method: _method, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("en")]
    [InlineData("pt-BR")]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(method: _method, request: request, culture: culture);

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