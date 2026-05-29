using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Domain.Entities;
using FluentAssertions;

namespace WebApi.Test.UserTask;

public class GetUserTasksByLoggedUserTest : EStudyClassFixture
{
	private readonly string _taskMethod = "user-tasks";
	private readonly string _token;

	public GetUserTasksByLoggedUserTest(CustomWebApplicationFactory factory) : base(factory)
	{
		_token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIdentifier());
	}

	[Fact]
	public async Task Success_WithFixedCategory()
	{
		var taskResponse = await DoPost(
			method: _taskMethod,
			request: new RequestCreateUserTaskJson
			{
				Title = "Estudar para a prova",
				Description = "Revisar os capitulos 1 e 2",
				DueDate = DateTime.UtcNow.AddDays(2),
				Category = CategoryEnum.Estudo
			},
			token: _token,
			culture: "pt-BR");

		taskResponse.StatusCode.Should().Be(HttpStatusCode.Created);

		var response = await DoGet(_taskMethod, token: _token, culture: "pt-BR");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		await using var responseBody = await response.Content.ReadAsStreamAsync();
		var responseData = await JsonDocument.ParseAsync(responseBody);

		var tasks = responseData.RootElement.EnumerateArray().ToList();
		tasks.Should().ContainSingle();

		var task = tasks.Single();
		task.GetProperty("title").GetString().Should().Be("Estudar para a prova");
		task.GetProperty("category").GetInt32().Should().Be((int)CategoryEnum.Estudo);
	}
}


