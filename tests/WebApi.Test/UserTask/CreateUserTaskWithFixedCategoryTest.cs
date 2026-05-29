using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Domain.Entities;
using FluentAssertions;

namespace WebApi.Test.UserTask;

public class CreateUserTaskWithFixedCategoryTest : EStudyClassFixture
{
    private readonly string _method = "user-tasks";
    private readonly string _token;

    public CreateUserTaskWithFixedCategoryTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIdentifier());
    }

    [Fact]
    public async Task Success_WithFixedCategory()
    {
        var response = await DoPost(
            method: _method,
            request: new RequestCreateUserTaskJson
            {
                Title = "Estudar para a prova",
                Description = "Revisar os capitulos 1 e 2",
                DueDate = DateTime.UtcNow.AddDays(2),
                Category = CategoryEnum.Estudo
            },
            token: _token,
            culture: "pt-BR");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().Should().Be("Estudar para a prova");
        responseData.RootElement.GetProperty("category").GetInt32().Should().Be((int)CategoryEnum.Estudo);
    }
}
