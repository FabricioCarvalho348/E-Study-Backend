using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.UserCustomCategory;

public class GetDefaultCategoriesTest : EStudyClassFixture
{
    private readonly string _method = "user-custom-categories/default-categories";
    private readonly string _token;

    public GetDefaultCategoriesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIdentifier());
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(_method, token: _token, culture: "pt-BR");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var categories = responseData.RootElement.EnumerateArray().ToList();
        categories.Should().HaveCount(10);

        categories.First().GetProperty("value").GetInt32().Should().Be(1);
        categories.First().GetProperty("name").GetString().Should().Be("Trabalho");

        categories.Last().GetProperty("value").GetInt32().Should().Be(10);
        categories.Last().GetProperty("name").GetString().Should().Be("Viagem");
    }
}
