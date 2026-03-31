using EStudy.Application.UseCases.Events.Create;
using EStudy.Application.UseCases.Events.Delete;
using EStudy.Application.UseCases.Events.GetAll;
using EStudy.Application.UseCases.Events.GetById;
using EStudy.Application.UseCases.Events.Patch;
using EStudy.Application.UseCases.Events.Update;
using EStudy.Communication.Requests.Events;
using EStudy.Communication.Responses.Errors;
using EStudy.Communication.Responses.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EStudy.Api.Controllers;

[Authorize]
public class EventController : EStudyBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateEventUseCase useCase,
        [FromBody] RequestCreateEventJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetEventByIdUseCase useCase,
        [FromRoute] long id)
    {
        var result = await useCase.Execute(id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseEventJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllEventsUseCase useCase,
        [BindRequired, FromQuery(Name = "start_date")] DateTime startDate,
        [BindRequired, FromQuery(Name = "end_date")] DateTime endDate,
        [FromQuery(Name = "tipo")] string? type)
    {
        var result = await useCase.Execute(startDate, endDate, type);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateEventUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestUpdateEventJson request)
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        [FromServices] IPatchEventUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestPatchEventJson request)
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteEventUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Execute(id);
        return NoContent();
    }
}


