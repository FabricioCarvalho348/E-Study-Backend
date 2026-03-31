using EStudy.Application.UseCases.UserTasks.Create;
using EStudy.Application.UseCases.UserTasks.Delete;
using EStudy.Application.UseCases.UserTasks.GetAll;
using EStudy.Application.UseCases.UserTasks.GetById;
using EStudy.Application.UseCases.UserTasks.Update;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Communication.Responses.Errors;
using EStudy.Communication.Responses.UserTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStudy.Api.Controllers;

[Authorize]
[Route("user-tasks")]
public class UserTaskController : EStudyBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserTaskJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateUserTaskUseCase useCase,
        [FromBody] RequestCreateUserTaskJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseUserTaskJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllUserTasksUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpGet("{taskId:long}")]
    [ProducesResponseType(typeof(ResponseUserTaskJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetUserTaskByIdUseCase useCase,
        [FromRoute] long taskId)
    {
        var result = await useCase.Execute(taskId);
        return Ok(result);
    }

    [HttpPut("{taskId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserTaskUseCase useCase,
        [FromRoute] long taskId,
        [FromBody] RequestUpdateUserTaskJson request)
    {
        await useCase.Execute(taskId, request);
        return NoContent();
    }

    [HttpDelete("{taskId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteUserTaskUseCase useCase,
        [FromRoute] long taskId)
    {
        await useCase.Execute(taskId);
        return NoContent();
    }
}

