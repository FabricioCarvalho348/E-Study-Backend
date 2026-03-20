using EStudy.Application.UseCases.User.ChangePassword;
using EStudy.Application.UseCases.User.Delete.Request;
using EStudy.Application.UseCases.User.Profile;
using EStudy.Application.UseCases.User.Register;
using EStudy.Application.UseCases.User.Update;
using EStudy.Communication.Requests.ChangePassword;
using EStudy.Communication.Requests.Users;
using EStudy.Communication.Responses.Errors;
using EStudy.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStudy.Api.Controllers;

public class UserController : EStudyBaseController
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
    
    [HttpGet("user-profile")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
    
    [HttpPut("update-profile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserProfile(
        [FromServices] IUpdateUserUseCase useCase, 
        [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
    
    [HttpPut("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase, 
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
    
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> Delete([FromServices] IRequestDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}