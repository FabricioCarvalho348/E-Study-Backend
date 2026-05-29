using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using EStudy.Communication.Requests.UserCustomCategories;
using EStudy.Communication.Requests.UserTasks;
using FluentAssertions;

namespace WebApi.Test.UserTask;

public class CreateUserTaskWithCategoryTest : EStudyClassFixture
{
    private readonly string _taskMethod = "user-tasks";
    private readonly string _categoryMethod = "user-custom-categories";
    private readonly string _token;

    public CreateUserTaskWithCategoryTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIdentifier());
    }

    [Fact]
    public async Task Success_WithCategory()
    {
        var categoryResponse = await DoPost(
            method: _categoryMethod,
            request: new RequestCreateUserCustomCategoryJson { Name = "Estudos" },
            token: _token,
            culture: "pt-BR");

        categoryResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var categoryBody = await categoryResponse.Content.ReadAsStreamAsync();
        var categoryData = await JsonDocument.ParseAsync(categoryBody);
        var categoryId = categoryData.RootElement.GetProperty("id").GetInt64();

        var taskResponse = await DoPost(
            method: _taskMethod,
            request: new RequestCreateUserTaskJson
            {
                Title = "Estudar para prova",
                Description = "Revisar todo o conteudo",
                CustomCategoryId = categoryId
            },
            token: _token,
            culture: "pt-BR");

        taskResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var taskBody = await taskResponse.Content.ReadAsStreamAsync();
        var taskData = await JsonDocument.ParseAsync(taskBody);

        taskData.RootElement.GetProperty("title").GetString().Should().Be("Estudar para prova");
        taskData.RootElement.GetProperty("customCategoryId").GetInt64().Should().Be(categoryId);
    }
}
