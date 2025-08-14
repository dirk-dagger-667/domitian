using domitian.Tests.Infrastructure.DataSources.UserAdmin.Controllers;
using domitian.Tests.Infrastructure.Extensions;
using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian_api.Tests.Fixtures.UserAdmin;
using FakeItEasy;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace domitian_api.Tests.UserAdmin
{
    public class RegisterControllerTester(RegisterControllerFixture _registerControllerFixture) : IClassFixture<RegisterControllerFixture>
    {
        [Fact]
        public async Task ConfirmRegistration_returns_ValidationProblem()
        {
            var fakeError = "FakeError";
            var fakeKey = "FakeKey";

            ConfirmRegistrationAsyncPipeline(
                errorKey: fakeKey,
                errorValue: fakeKey);

            var result = await _registerControllerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ActionResultAssertions.IsValidationProblem(result, fakeKey);
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_returns_BadRequest()
        {
            var error = Error.Exception;

            ConfirmRegistrationAsyncPipeline(
                Result<string>.Failure(string.Empty, ResultType.BadRequest, error),
                new BadRequestObjectResult(error.Message));

            var result = await _registerControllerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ActionResultAssertions.IsBadRequest(result, error.Message!);
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_returns_Unauthorized()
        {
            var notFoundResult = Result<string>.Failure(string.Empty, ResultType.Unauthorized, LoginErrors.LoginNotFound);

            ConfirmRegistrationAsyncPipeline(
                notFoundResult,
                new NotFoundObjectResult(notFoundResult.Error?.Message));

            var result = await _registerControllerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ActionResultAssertions.IsNotFound(result, notFoundResult.Error?.Message!);
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_returns_OK()
        {
            var fakeCallbackUrl = "FakeUrl";

            ConfirmRegistrationAsyncPipeline(
                Result<string>.Success(fakeCallbackUrl),
                new OkObjectResult(fakeCallbackUrl));

            var result = await _registerControllerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ActionResultAssertions.IsOk(result, fakeCallbackUrl);
        }

        [Fact]
        public async Task RegisterAsync_should_return_ValidationProblem()
        {
            var fakeKey = "FakeKey";
            var fakeError = "FakeError";

            //Failing fake validation result
            var fakeValidationResult = new FluentValidation.Results.ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure(fakeKey, fakeError) } };

            ArrangeRegisterAsyncPipeline(fakeValidationResult);

            var result = await _registerControllerFixture.SUT.RegisterAsync(_registerControllerFixture.RegReqValidator, A.Dummy<RegisterRequest>());

            ActionResultAssertions.IsValidationProblem(result, fakeError);
        }

        [Theory]
        [MemberData(nameof(RegisterControllerTestData.RegisterAsyncBadRequestErrors), MemberType = typeof(RegisterControllerTestData))]
        public async Task RegisterAsync_ashould_return_BadRequest(Error err)
        {
            ArrangeRegisterAsyncPipeline(
                new ValidationResult(),
                Result<string>.Failure(string.Empty, ResultType.BadRequest, err),
                new BadRequestObjectResult(err.Message));

            var result = await _registerControllerFixture.SUT.RegisterAsync(_registerControllerFixture.RegReqValidator, A.Dummy<RegisterRequest>());

            ActionResultAssertions.IsBadRequest(result, err.Message);
        }

        [Theory]
        [InlineData("FakeUri", "FakeData")]
        [InlineData("", "")]
        public async Task RegisterAsync_should_return_Created(string iru, string data)
        {
            ArrangeRegisterAsyncPipeline(
                new ValidationResult(),
                Result<string>.Success(data),
                new CreatedResult(iru, data));

            var result = await _registerControllerFixture.SUT.RegisterAsync(_registerControllerFixture.RegReqValidator, A.Dummy<RegisterRequest>());

            ActionResultAssertions.IsCreated(result, data);
        }

        [Fact]
        public async Task ConfirmEmailAsync_should_return_ValidationProblem()
        {
            var invalidRequest = new ConfirmEmailRequest() { UserId = "FakeId", Code = "FakeId" };

            var fakeValidationResult = new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("FakeKey", ValidationErrorsMessages.CriticalError) } };

            ArrangeConfirmEmailAsyncPipeline(fakeValidationResult);

            var result = await _registerControllerFixture.SUT.ConfirmEmailAsync(_registerControllerFixture.ConEmailReqValidator, invalidRequest.UserId, invalidRequest.Code);

            ActionResultAssertions.IsValidationProblem(result, ValidationErrorsMessages.CriticalError);
        }

        [Theory]
        [MemberData(nameof(RegisterControllerTestData.ConfirmRegistrationAsyncBadRequestErrors), MemberType = typeof(RegisterControllerTestData))]
        public async Task ConfirmEmailAsync_should_return_BadRequest(Error err)
        {
            ArrangeConfirmEmailAsyncPipeline(
                new ValidationResult(),
                Result.Failure(string.Empty, ResultType.BadRequest, err),
                new BadRequestObjectResult(err.Message));

            var result = await _registerControllerFixture.SUT.ConfirmEmailAsync(_registerControllerFixture.ConEmailReqValidator, A.Dummy<string>(), A.Dummy<string>());

            ActionResultAssertions.IsBadRequest(result, err.Message!);
        }

        [Fact]
        public async Task ConfirmEmailAsync_should_return_NotFound()
        {
            var error = LoginErrors.LoginNotFound;

            ArrangeConfirmEmailAsyncPipeline(
                new ValidationResult(),
                Result.Failure(string.Empty, ResultType.NotFound, error),
                new BadRequestObjectResult(error.Message));

            var result = await _registerControllerFixture.SUT.ConfirmEmailAsync(_registerControllerFixture.ConEmailReqValidator, A.Dummy<string>(), A.Dummy<string>());

            ActionResultAssertions.IsBadRequest(result, error.Message!);
        }

        [Fact]
        public async Task ConfirmEmailAsync_should_return_Ok()
        {
            ArrangeConfirmEmailAsyncPipeline(
                new ValidationResult(),
                Result.Success(),
                new OkObjectResult(true));

            var result = await _registerControllerFixture.SUT.ConfirmEmailAsync(_registerControllerFixture.ConEmailReqValidator, A.Dummy<string>(), A.Dummy<string>());

            ActionResultAssertions.IsOk(result, true);
        }

        private void ConfirmRegistrationAsyncPipeline(
            Result<string>? conRegRes = null,
            ObjectResult? actionRes = null,
            string? errorKey = null,
            string? errorValue = null)
        {
            if (conRegRes != null)
            {
                A.CallTo(() => _registerControllerFixture.RegisterService.ConfirmRegistrationAsync(A<string>.Ignored))
                .Returns(conRegRes);
            }

            if (actionRes != null)
            {
                A.CallTo(() => _registerControllerFixture.ReturnResultsHelper.ResultTypeToActionResult(A<Result<string>>.Ignored))
                .Returns(actionRes);
            }

            _registerControllerFixture.SUT.ModelState.Clear();

            if (errorKey is not null && errorValue is not null)
                _registerControllerFixture.SUT.ModelState.AddModelError(errorKey, errorValue);
        }

        private void ArrangeRegisterAsyncPipeline(
            ValidationResult? valRes = null,
            Result<string>? regRes = null,
            ObjectResult? actionRes = null)
        {
            if (valRes != null)
                A.CallTo(() => _registerControllerFixture.RegReqValidator.ValidateAsync(A<FluentValidation.ValidationContext<RegisterRequest>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(valRes);

            if (regRes != null)
                A.CallTo(() => _registerControllerFixture.RegisterService.RegisterAsync(A<RegisterRequest>.Ignored))
                .Returns(regRes);

            if (actionRes != null)
                A.CallTo(() => _registerControllerFixture.ReturnResultsHelper.ResultTypeToActionResult(A<Result<string>>.Ignored))
                .Returns(actionRes);
        }

        private void ArrangeConfirmEmailAsyncPipeline(
            ValidationResult? valRes = null,
            Result? confEmailRes = null,
            ObjectResult? actionRes = null)
        {
            if (valRes != null)
            {
                A.CallTo(() => _registerControllerFixture.ConEmailReqValidator.ValidateAsync(A<FluentValidation.ValidationContext<ConfirmEmailRequest>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(valRes);
            }

            if (confEmailRes != null)
            {
                A.CallTo(() => _registerControllerFixture.RegisterService.ConfirmEmailAsync(A<ConfirmEmailRequest>.Ignored))
                .Returns(confEmailRes);
            }

            if (actionRes != null)
            {
                A.CallTo(() => _registerControllerFixture.ReturnResultsHelper.ResultTypeToActionResultBase(A<Result>.Ignored))
                .Returns(actionRes);
            }
        }
    }
}
