using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian.Tests.Infrastructure.DataSources.UserAdmin.Services;
using domitian.Tests.Infrastructure.Dtos;
using domitian.Tests.Infrastructure.Extensions;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace domitian.Business.Tests.Services.UserAdmin.Registration
{
    public class RegisterServiceTester(RegisterServiceFixture _regSerFixture) : IClassFixture<RegisterServiceFixture>
    {
        [Theory]
        [MemberData(nameof(RegisterServiceTestData.RegisterAsyncBadRequestParams), MemberType = typeof(RegisterServiceTestData))]
        public async Task RegisterAsync_returns_BadRequest_Error(RegisterBadReqDto dto)
        {
            var fakeFailureResult = IdentityResult.Failed(A.Dummy<IdentityError>());

            ArrangeRegisterPipelineAsync(dto.User, dto.IdRes, dto.SupportsEmail, dto.AddToRoleRes);

            var registerResult = await _regSerFixture.SUT.RegisterAsync(A.Dummy<RegisterRequest>());

            ResultAssertions.IsBadRequest(registerResult, dto.Error);
        }

        [Fact]
        public void RegisterAsync_throws_Exception()
        {
            A.CallTo(() => _regSerFixture.UserManager.FindByEmailAsync(A<string>.Ignored))
                .ThrowsAsync(new Exception());

            FluentActions.Invoking(async () => await _regSerFixture.SUT.RegisterAsync(A.Dummy<RegisterRequest>()));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RegisterAsync_should_return_Success_with_callbackUrl(bool reqConfAccount)
        {
            var fakeSuccessResult = IdentityResult.Success;
            _regSerFixture.UserManager.Options.SignIn.RequireConfirmedAccount = reqConfAccount;

            ArrangeRegisterPipelineAsync(null, fakeSuccessResult, true, fakeSuccessResult);

            var registerResult = await _regSerFixture.SUT.RegisterAsync(A.Dummy<RegisterRequest>());

            ResultAssertions.IsCreated(registerResult);

            if (reqConfAccount)
            {
                registerResult.As<Result<string>>().Should().NotBeNull();
                registerResult.As<Result<string>>().Data.Should().NotBeNullOrWhiteSpace().And.ContainAll(ControllerEndpPoints.RegisterController, ControllerEndpPoints.ConfirmEmail, "userId=", "&code=");
            }
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_returns_Unautorized()
        {
            DomitianIDUser? fakeUser = null;

            A.CallTo(() => _regSerFixture.UserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(fakeUser);

            var result = await _regSerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ResultAssertions.IsUnauthorized(result);
        }

        [Fact]
        public void ConfirmRegistrationAsync_throws_Exception()
        {
            A.CallTo(() => _regSerFixture.UserManager.FindByEmailAsync(A<string>.Ignored))
                .ThrowsAsync(new Exception());

            FluentActions.Invoking(async () => await _regSerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>()));
        }

        [Fact]
        public async Task ConfirmRegistrationAsync_should_return_Success_with_Data()
        {
            A.CallTo(() => _regSerFixture.UserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(A.Dummy<DomitianIDUser>());

            var result = await _regSerFixture.SUT.ConfirmRegistrationAsync(A.Dummy<string>());

            ResultAssertions.IsOkData(result);

            result.As<Result<string>>().Data.Should().NotBeNullOrWhiteSpace().And.ContainAll(ControllerEndpPoints.RegisterController, ControllerEndpPoints.ConfirmEmail, "userId=", "&code=");
        }

        [Theory]
        [MemberData(memberName: nameof(RegisterServiceTestData.ConfirmEmailFailureParams), MemberType = typeof(RegisterServiceTestData))]
        public async Task ConfirmEmailAsync_return_Fails(ConfirmEmailFailureDto dto)
        {
            ArrangeConfirmEmailAsyncPipeline(dto.User, dto.IdRes);

            var result = await _regSerFixture.SUT.ConfirmEmailAsync(dto.ConfEmailReq);

            dto?.Assertion(result, dto.Error);
        }

        [Fact]
        public async Task ConfirmEmailAsync_should_return_Success_no_Data()
        {
            var request = new ConfirmEmailRequest()
            {
                UserId = A.Dummy<string>(),
                Code = A.Dummy<string>()
            };

            ArrangeConfirmEmailAsyncPipeline(
                A.Dummy<DomitianIDUser>(),
                IdentityResult.Success);

            var result = await _regSerFixture.SUT.ConfirmEmailAsync(request);

            ResultAssertions.IsOk(result);
        }

        private void ArrangeRegisterPipelineAsync(
            DomitianIDUser? DomitianIDUser = null,
            IdentityResult? createRes = null,
            bool supportsUserEmail = false,
            IdentityResult? addToRoleRes = null)
        {
            A.CallTo(() => _regSerFixture.UserManager.FindByEmailAsync(A<string>.Ignored))
            .Returns(DomitianIDUser);

            if (createRes != null)
                A.CallTo(() => _regSerFixture.UserManager.CreateAsync(A<DomitianIDUser>.Ignored, A<string>.Ignored))
                .Returns(createRes);

            A.CallTo(() => _regSerFixture.UserManager.SupportsUserEmail)
                .Returns(supportsUserEmail);

            if (addToRoleRes != null)
                A.CallTo(() => _regSerFixture.UserManager.AddToRoleAsync(A<DomitianIDUser>.Ignored, A<string>.Ignored))
                .Returns(addToRoleRes);
        }

        private void ArrangeConfirmEmailAsyncPipeline(
            DomitianIDUser? DomitianIDUser = null,
            IdentityResult? confEmailRes = null)
        {
            A.CallTo(() => _regSerFixture.UserManager.FindByIdAsync(A<string>.Ignored))
                .Returns(DomitianIDUser);

            if (confEmailRes != null)
                A.CallTo(() => _regSerFixture.UserManager.ConfirmEmailAsync(A<DomitianIDUser>.Ignored, A<string>.Ignored))
                .Returns(confEmailRes);
        }
    }
}
