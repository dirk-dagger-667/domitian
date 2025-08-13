using domitian.Business.Contracts;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian_api.Constants;
using domitian_api.Extensions;
using domitian_api.Helpers;
using domitian_api.Infrastructure.Constants;
using domitian_api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace domitian_api.Controllers.UserAdmin
{
  [ApiController]
  [AllowAnonymous]
  [Route(ApiPathConstants.Login)]
  public class LoginController(
    [FromKeyedServices(AppConstants.CrossCuttingKey)] ILoginService _loginService,
      IReturnResultsHelper _returnResultsHelper) : ControllerBase
  {
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromServices] LoginRequestValidator loginValidator,
        [FromBody] LoginRequest request)
    {
      var valRslt = await loginValidator.ValidateAsync(request);

      if (!valRslt.IsValid)
        return base.ValidationProblem(valRslt.Errors.ConcatErrors());

      var loginRslt = await _loginService.LoginAsync(request);

      return _returnResultsHelper.ResultTypeToActionResult(loginRslt);
    }

    [HttpPost(ApiPathConstants.Refresh)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshAsync([FromServices] RefreshRequestValidator refResValidator, [FromBody] RefreshRequest request)
    {
      var valRes = await refResValidator.ValidateAsync(request);

      if (!valRes.IsValid)
        return base.ValidationProblem(valRes.Errors.ConcatErrors());

      var refRes = await _loginService.RefreshAccessAsync(request);

      return _returnResultsHelper.ResultTypeToActionResult(refRes);
    }

    [HttpGet($"{ApiPathConstants.RevokeControllerPath}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeAccess([Required][EmailAddress] string email)
    {
      if (!ModelState.IsValid)
        return base.ValidationProblem(ModelState.GetErrorsAsString());

      var revRes = await _loginService.RevokeAccessAsync(email);

      return _returnResultsHelper.ResultTypeToActionResultBase(revRes);
    }
  }
}
