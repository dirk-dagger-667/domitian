using domitian.Business.Contracts;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian_api.Constants;
using domitian_api.Extensions;
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
    [FromKeyedServices(AppConstants.CrossCuttingKey)] ILoginService loginService )
    : ControllerBase
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

      var loginRslt = await loginService.LoginAsync(request);

      return loginRslt.ToActionResultWithData(HttpContext);
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

      var refRes = await loginService.RefreshAccessAsync(request);

      return refRes.ToActionResultWithData(HttpContext);
    }

    [HttpGet($"{ApiPathConstants.RevokeControllerPath}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeAccess([Required][EmailAddress] string email)
    {
      if (!ModelState.IsValid)
        return base.ValidationProblem(ModelState.GetErrorsAsString());

      var revRes = await loginService.RevokeAccessAsync(email);

      return revRes.ToActionResult(HttpContext);
    }
  }
}
