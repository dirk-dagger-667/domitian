using domitian.Business.Contracts;
using domitian_api.Extensions;
using domitian_api.Helpers;
using domitian.Infrastructure.Validators;
using domitian.Models.Requests.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace domitian_api.Controllers.UserAdmin
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/register")]
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

        [HttpGet("confirm-email/{userId}/{code}")]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromServices] ConfirmEmailRequestValidator conEmailReqValidator,
            string userId,
            string code
          )
        {
            var request = new ConfirmEmailRequest() { UserId = userId, Code = code };

            var validationResult = await conEmailReqValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return base.ValidationProblem(validationResult.Errors.ConcatErrors());
            }

            var confirmEmailResult = await _registerService.ConfirmEmailAsync(request);

            return _returnResultsHelper.ResultTypeToActionResultBase(confirmEmailResult);
        }

        [HttpPost("confirm-registration/{email}")]
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
