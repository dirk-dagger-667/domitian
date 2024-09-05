using dominitian.Tests.Infrastructure.DataSources.UserAdmin.Services;
using dominitian.Tests.Infrastructure.Dtos;
using dominitian.Tests.Infrastructure.Extensions;
using dominitian_ui.Models.Requests.Login;
using domition_api.Data.Identity;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace dominitian.Business.Tests.Services.UserAdmin.Login
{
    public class LoginServiceTester(LoginServiceFixture _loginServiceFixture) : IClassFixture<LoginServiceFixture>
    {
        [Theory]
        [MemberData(memberName: nameof(LoginServiceTestData.LoginBadRequestParams), MemberType = typeof(LoginServiceTestData))]
        public async Task LoginAsync_returns_BadRequest(LoginBadRequestDto dto)
        {
            ArrangeLoginAsyncPipeline(dto.User,
                dto.CheckPassRes,
                dto.SignInRes,
                dto.PassSignInthrowsEx);

            var loginResult = await _loginServiceFixture.SUT.LoginAsync(A.Dummy<LoginRequest>());

            ResultAssertions.IsBadRequest(loginResult, dto.Error);
        }

        [Theory]
        [MemberData(memberName: nameof(LoginServiceTestData.LoginUnauthorizedParams), MemberType = typeof(LoginServiceTestData))]
        public async Task LoginAsync_returns_Unauthorized(LoginFailureDto dto)
        {
            ArrangeLoginAsyncPipeline(dto.User,
                dto.CheckPassRes,
                dto.SignInRes,
                dto.PassSignInthrowsEx);

            var loginResult = await _loginServiceFixture.SUT.LoginAsync(A.Dummy<LoginRequest>());

            ResultAssertions.IsUnauthorized(loginResult);
        }

        [Fact]
        public async Task LoginAsync_returns_Success()
        {
            ArrangeLoginAsyncPipeline(
                A.Dummy<DominitianIDUser>(),
                true,
                SignInResult.Success);

            var loginResult = await _loginServiceFixture.SUT.LoginAsync(A.Dummy<LoginRequest>());

            ResultAssertions.IsOkData(loginResult);
        }

        [Theory]
        [MemberData(memberName: nameof(LoginServiceTestData.RefreshAccessUnauthorizedParams), MemberType = typeof(LoginServiceTestData))]
        public async Task RefreshAccessAsync_returns_Unauthorized(RefreshAccessBaseDto dto)
        {
            ArrangeRefreshAccessAsyncPipeline(dto.ClaimsPrincipal, dto.User, dto.RefReq?.AccessToken);

            var result = await _loginServiceFixture.SUT.RefreshAccessAsync(dto.RefReq!);

            ResultAssertions.IsUnauthorized(result);
        }

        [Fact]
        public async Task RefreshAccessAsync_returns_BadRequest_Exception()
        {
            var fake = "fake";
            var refReq = new RefreshRequest()
            {
                AccessToken = fake,
                RefreshToken = fake
            };

            A.CallTo(() => _loginServiceFixture.TokenService.GetPrincipalFromExpiredToken(A<string>.Ignored))
                .Throws<Exception>();

            var result = await _loginServiceFixture.SUT.RefreshAccessAsync(refReq);

            ResultAssertions.IsException(result);
        }

        [Fact]
        public async Task RefreshAccessAsync_returns_Success()
        {
            var fake = "fake";

            var user = new DominitianIDUser()
            {
                RefreshToken = fake,
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5),
                Email = fake
            };

            var claimsPrincipalFake = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, fake)
                    }));

            var refReq = new RefreshRequest()
            {
                AccessToken = fake,
                RefreshToken = fake
            };

            ArrangeRefreshAccessAsyncPipeline(
                claimsPrincipalFake,
                user,
                fake);

            var result = await _loginServiceFixture.SUT.RefreshAccessAsync(refReq);

            ResultAssertions.IsOkData(result);
            result.Data!.BearerToken.Should().NotBeNullOrWhiteSpace()
                .And.Be(fake);
            result.Data!.Email.Should().NotBeNullOrWhiteSpace()
                .And.Be(fake);
            result.Data!.RefreshToken.Should().NotBeNullOrWhiteSpace()
                .And.Be(fake);
        }

        [Fact]
        public async Task RevokeAccessAsync_returns_Success()
        {
            ArrangeRevokeAccessAsync(A.Dummy<DominitianIDUser>(),
            IdentityResult.Success);

            var result = await _loginServiceFixture.SUT.RevokeAccessAsync(A.Dummy<string>());

            ResultAssertions.IsOk(result);
        }

        [Fact]
        public async Task RevokeAccessAsync_returns_BadRequest_Exception()
        {
            A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.FindByNameAsync(A<string>.Ignored))
                .Throws(new Exception());

            var result = await _loginServiceFixture.SUT.RevokeAccessAsync(A.Dummy<string>());

            ResultAssertions.IsException(result);
        }

        [Theory]
        [MemberData(memberName: nameof(LoginServiceTestData.RevokeAccessFailureParams), MemberType = typeof(LoginServiceTestData))]
        public async Task RevokeAccessAsync_returns_Unauthorized(RevokeAccessUnauthorizedDto dto)
        {
            ArrangeRevokeAccessAsync(dto.User, dto.UpdRes);

            var result = await _loginServiceFixture.SUT.RevokeAccessAsync(dto.UserName);

            ResultAssertions.IsUnauthorized(result);
        }

        private void ArrangeRevokeAccessAsync(DominitianIDUser? user = null,
            IdentityResult? updRes = null)
        {
                A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.FindByNameAsync(A<string>.Ignored))
                .Returns(user);

                A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.UpdateAsync(A<DominitianIDUser>.Ignored))
                .Returns(updRes);
        }

        private void ArrangeRefreshAccessAsyncPipeline(
            ClaimsPrincipal? claimsPrincipal = null,
            DominitianIDUser? user = null,
            string? jwt = null)
        {
            if (claimsPrincipal is not null)
                A.CallTo(() => _loginServiceFixture.TokenService.GetPrincipalFromExpiredToken(A<string>.Ignored))
                    .Returns(claimsPrincipal);

            if (user is not null)
                A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.FindByNameAsync(A<string>.Ignored))
                    .Returns(user);

            if (jwt is not null)
                A.CallTo(() => _loginServiceFixture.TokenService.GenerateJwt(A<DominitianIDUser>.Ignored))
                            .Returns(jwt);
        }

        private void ArrangeLoginAsyncPipeline(
            DominitianIDUser? dominitianIDUser = null,
            bool checkPassRes = false,
            SignInResult? signInRes = null,
            bool passSignInThrowsEx = false)
        {
            A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(dominitianIDUser);

            A.CallTo(() => _loginServiceFixture.SignInManager.UserManager.CheckPasswordAsync(A<DominitianIDUser>.Ignored, A<string>.Ignored))
                .Returns(checkPassRes);

            if (signInRes != null)
            {
                A.CallTo(() => _loginServiceFixture.SignInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored))
                .Returns(signInRes);
                return;
            }

            if (passSignInThrowsEx)
            {
                A.CallTo(() => _loginServiceFixture.SignInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored))
                .Throws<Exception>();
            }
        }
    }
}
