using dominitian.Business.Contracts;
using dominitian_api.Extensions;
using dominitian_api.Helpers;
using dominitian_ui.Infrastructure.Validators;
using dominitian_ui.Models.Requests.Registration;
using domition_api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace domition_api.Controllers.UserAdmin
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class RegisterController(IRegisterService _registerService,
        IReturnResultsHelper _returnResultsHelper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<string>(StatusCodes.Status201Created)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(
            [FromServices] RegisterRequestValidator regReqValidator,
            [FromBody] RegisterRequest request)
        {
            var validationResult = await regReqValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return base.ValidationProblem(validationResult.Errors.ConcatErrors());
            }

            var registerResult = await _registerService.RegisterAsync(request);

            return _returnResultsHelper.ResultTypeToActionResult(registerResult);
        }

        [HttpGet]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromServices] ConfirmEmailRequestValidator conEmailReqValidator,
            [FromQuery(Name = "userId")] string userId,
            [FromQuery(Name = "code")] string code)
        {
            var request = new ConfirmEmailRequest() { UserId = userId, Code = code };

            var validationResult = await conEmailReqValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return base.ValidationProblem(validationResult.Errors.ConcatErrors());
            }

            var confirmEmailResult = await _registerService.ConfirmEmailAsync(request);

            return _returnResultsHelper.ResultTypeToActionResult(confirmEmailResult);
        }

        [HttpPost("{email}")]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmRegistrationAsync([Required][EmailAddress] string email)
        {
            if (!ModelState.IsValid)
            {
                return base.ValidationProblem(ModelState.GetErrorsAsString());
            }

            var conRegResult = await _registerService.ConfirmRegistrationAsync(email);

            return _returnResultsHelper.ResultTypeToActionResult(conRegResult);
        }
    }
}
