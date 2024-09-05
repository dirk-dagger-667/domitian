using dominitian.Tests.Infrastructure.DataSources.UserAdmin.Controllers;
using dominitian.Tests.Infrastructure.Extensions;
using dominitian_ui.Models.Requests.Login;
using dominitian_ui.Models.Responses.Login;
using dominitian_ui.Models.Results;
using domitian_api.Tests.Fixtures.UserAdmin;
using FakeItEasy;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using LoginModels = dominitian_ui.Models.Requests.Login;

namespace domitian_api.Tests.UserAdmin
{
    public class LoginControllerTester(LoginControllerFixture _loginControllerFixture) : IClassFixture<LoginControllerFixture>
    {
        [Fact]
        public async Task LoginAsync_returns_ValidationProblem()
        {
            //Failing fake validation result
            var fakeValidationResult = new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("FakeKey", ValidationErrorsMessages.CriticalError) } };

            ArrangeLoginAsyncPipeline(fakeValidationResult);

            var result = await _loginControllerFixture.SUT.LoginAsync(_loginControllerFixture.LogResVal, A.Dummy<LoginModels.LoginRequest>());

            ActionResultAssertions.IsValidationProblem(result, ValidationErrorsMessages.CriticalError);
        }

        [Theory]
        [MemberData(nameof(LoginControllerTestData.FailingResultsBadRequest), MemberType = typeof(LoginControllerTestData))]
        public async Task LoginAsync_returns_BadRequest(Result<LoginResponse> serviceResult)
        {
            ArrangeLoginAsyncPipeline(
                new ValidationResult(),
                serviceResult,
                new BadRequestObjectResult(serviceResult.Error?.Message));

            var result = await _loginControllerFixture.SUT.LoginAsync(_loginControllerFixture.LogResVal, A.Dummy<LoginModels.LoginRequest>());

            ActionResultAssertions.IsBadRequest(result, serviceResult.Error?.Message!);
        }

        [Fact]
        public async Task LoginAsync_returns_NotFound()
        {
            var notFoundError = LoginErrors.LoginNotFound("asd@asd.com");
            var failingResult = Result<LoginResponse>.Failure(ResultTypes.NotFound, notFoundError);

            ArrangeLoginAsyncPipeline(
                new ValidationResult(),
                failingResult,
                new NotFoundObjectResult(notFoundError.Message));

            var result = await _loginControllerFixture.SUT.LoginAsync(_loginControllerFixture.LogResVal, A.Dummy<LoginModels.LoginRequest>());

            ActionResultAssertions.IsNotFound(result, notFoundError.Message!);
        }

        [Fact]
        public async Task LoginAsync_returns_Ok()
        {
            var fake = "fake";

            var fakeSuccessReponse = new LoginResponse() { BearerToken = fake, Email = fake, RefreshToken = fake };

            ArrangeLoginAsyncPipeline(
                new ValidationResult(),
                Result<LoginResponse>.Success(fakeSuccessReponse),
                new OkObjectResult(fakeSuccessReponse));

            var result = await _loginControllerFixture.SUT.LoginAsync(_loginControllerFixture.LogResVal, A.Dummy<LoginModels.LoginRequest>());

            ActionResultAssertions.IsOk(result, fakeSuccessReponse);
        }

        [Fact]
        public async Task RefreshAsync_returns_ValidationProblem()
        {
            var fakeValidationResult = new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("FakeKey", ValidationErrorsMessages.CriticalError) } };

            ArrangeRefreshAsyncPipeline(fakeValidationResult);

            var result = await _loginControllerFixture.SUT.RefreshAsync(_loginControllerFixture.RefReqVal, A.Dummy<LoginModels.RefreshRequest>());

            ActionResultAssertions.IsValidationProblem(result, ValidationErrorsMessages.CriticalError);
        }

        [Fact]
        public async Task RefreshAsync_returns_BadRequest()
        {
            var exceptionMessage = "fake exception";
            var refRes = Result<LoginResponse>.Failure(new Exception(exceptionMessage));

            ArrangeRefreshAsyncPipeline(
                new ValidationResult(),
                Result<LoginResponse>.Failure(ResultTypes.Unauthorized),
                new BadRequestObjectResult(exceptionMessage));

            var result = await _loginControllerFixture.SUT.RefreshAsync(_loginControllerFixture.RefReqVal, A.Dummy<RefreshRequest>());

            ActionResultAssertions.IsBadRequest(result, exceptionMessage);
        }

        [Fact]
        public async Task RefreshAsync_returns_Unauthorized()
        {
            ArrangeRefreshAsyncPipeline(
                new ValidationResult(),
                Result<LoginResponse>.Failure(ResultTypes.Unauthorized),
                new UnauthorizedResult());

            var result = await _loginControllerFixture.SUT.RefreshAsync(_loginControllerFixture.RefReqVal, A.Dummy<RefreshRequest>());

            ActionResultAssertions.IsUnauthorized(result);
        }

        private void ArrangeRefreshAsyncPipeline(
            ValidationResult? valRes = null,
            Result<LoginResponse>? refRes = null,
            IActionResult? actionRes = null)
        {
            if (valRes is not null)
            {
                A.CallTo(() => _loginControllerFixture.RefReqVal.ValidateAsync(A<FluentValidation.ValidationContext<LoginModels.RefreshRequest>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(valRes);
            }

            if (refRes is not null)
            {
                A.CallTo(() => _loginControllerFixture.LoginService.RefreshAccessAsync(A<RefreshRequest>.Ignored))
                .Returns(refRes);
            }

            if (actionRes is not null)
            {
                A.CallTo(() => _loginControllerFixture.ReturnResultsHelper.ResultTypeToActionResult(A<Result<LoginResponse>>.Ignored))
                .Returns(actionRes);
            }
        }

        private void ArrangeLoginAsyncPipeline(
            ValidationResult? valRes = null,
            Result<LoginResponse>? logRes = null,
            ObjectResult? actionRes = null)
        {
            if (valRes != null)
            {
                A.CallTo(() => _loginControllerFixture.LogResVal.ValidateAsync(A<FluentValidation.ValidationContext<LoginModels.LoginRequest>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(valRes);
            }

            if (logRes != null)
            {
                A.CallTo(() => _loginControllerFixture.LoginService.LoginAsync(A<LoginModels.LoginRequest>.Ignored))
                .Returns(logRes);
            }

            if (actionRes != null)
            {
                A.CallTo(() => _loginControllerFixture.ReturnResultsHelper.ResultTypeToActionResult(A<Result<LoginResponse>>.Ignored))
                .Returns(actionRes);
            }
        }
    }
}
