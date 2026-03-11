using EStudy.Application.UseCases.Login.DoLogin;
using EStudy.Communication.Requests.DoLogin;
using EStudy.Communication.Responses.Errors;
using EStudy.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStudy.Api.Controllers;

public class LoginController : EStudyBaseController
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);
        
        return Ok(response);
    }
}