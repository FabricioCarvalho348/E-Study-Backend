using EStudy.Application.UseCases.UserCustomCategories.Create;
using EStudy.Application.UseCases.UserCustomCategories.Delete;
using EStudy.Application.UseCases.UserCustomCategories.GetAll;
using EStudy.Application.UseCases.UserCustomCategories.Update;
using EStudy.Communication.Requests.UserCustomCategories;
using EStudy.Communication.Responses.UserCustomCategories;
using EStudy.Communication.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStudy.Api.Controllers;

[Authorize]
[Route("user-custom-categories")]
public class UserCustomCategoryController : EStudyBaseController
{
    [HttpGet("default-categories")]
    [ProducesResponseType(typeof(List<ResponseCategoryEnumJson>), StatusCodes.Status200OK)]
    public IActionResult GetDefaultCategories()
    {
        var result = ResponseCategoryEnumJson.GetAll();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseUserCustomCategoryJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateUserCustomCategoryUseCase useCase,
        [FromBody] RequestCreateUserCustomCategoryJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ResponseUserCustomCategoryJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllUserCustomCategoriesUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut("{categoryId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserCustomCategoryUseCase useCase,
        [FromRoute] long categoryId,
        [FromBody] RequestUpdateUserCustomCategoryJson request)
    {
        await useCase.Execute(categoryId, request);
        return NoContent();
    }

    [HttpDelete("{categoryId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteUserCustomCategoryUseCase useCase,
        [FromRoute] long categoryId)
    {
        await useCase.Execute(categoryId);
        return NoContent();
    }
}

