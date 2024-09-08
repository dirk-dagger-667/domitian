using dominitian.Business.Contracts;
using dominitian.Infrastructure.Validators;
using dominitian_api.Extensions;
using dominitian_api.Helpers;
using dominitian_ui.Models.Requests.Login;
using dominitian_ui.Models.Responses.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace domition_api.Controllers.UserAdmin
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class LoginController(ILoginService _loginService,
        IReturnResultsHelper _returnResultsHelper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<bool>(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
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

        [HttpPost]
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

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeAccess()
        {
            var username = HttpContext.User.Identity?.Name;

            var revRes = await _loginService.RevokeAccessAsync(username);

            return _returnResultsHelper.ResultTypeToActionResult(revRes);
        }
    }
}
