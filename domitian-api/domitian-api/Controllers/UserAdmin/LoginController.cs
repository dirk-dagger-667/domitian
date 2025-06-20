using domitian_api.Extensions;
using domitian_api.Helpers;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using domitian.Business.Contracts;
using domitian.Infrastructure.Validators;

namespace domitian_api.Controllers.UserAdmin
{
  [ApiController]
  [AllowAnonymous]
  [Route("api/login")]
  public class LoginController(ILoginService _loginService,
      IReturnResultsHelper _returnResultsHelper) : ControllerBase
  {
    [HttpPost]
    [ProducesResponseType<bool>(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status500InternalServerError)]
    //[Produces("application/xml", "text/xml", "application/problem+xml")]
    public async Task<IActionResult> LoginAsync([FromServices] LoginRequestValidator loginValidator,
        [FromBody] LoginRequest request)
    {
      var valRslt = await loginValidator.ValidateAsync(request);

      if (!valRslt.IsValid)
      {
        return base.ValidationProblem(valRslt.Errors.ConcatErrors());
      }

      var loginRslt = await _loginService.LoginAsync(request);

      return _returnResultsHelper.ResultTypeToActionResult(loginRslt);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshAsync([FromServices] RefreshRequestValidator refResValidator, [FromBody] RefreshRequest request)
    {
      var valRes = await refResValidator.ValidateAsync(request);

      if (!valRes.IsValid)
      {
        return base.ValidationProblem(valRes.Errors.ConcatErrors());
      }

      var refRes = await _loginService.RefreshAccessAsync(request);

      return _returnResultsHelper.ResultTypeToActionResult(refRes);
    }

    [HttpPatch("revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeAccess()
    {
      var username = HttpContext.User.Identity?.Name;

      var revRes = await _loginService.RevokeAccessAsync(username);

      return _returnResultsHelper.ResultTypeToActionResultBase(revRes);
    }
  }
}
