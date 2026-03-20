using EStudy.Application.UseCases.Token;
using EStudy.Communication.Requests.Tokens;
using EStudy.Communication.Responses.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace EStudy.Api.Controllers;

public class TokenController : EStudyBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}