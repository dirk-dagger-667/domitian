using domitian.Business.Contracts;
using domitian.Models.Requests.Login;
using domitian.Models.Requests.Registration;
using domitian.Models.Responses.Login;
using domitian.Models.Results;
using domitian.Tests.Integration.DataSources;
using domitian.Tests.Integration.Fixture;
using domitian_api.Data.Identity;
using domitian_api.Infrastructure.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace domitian.Tests.Integration.Controllers
{
  [Collection("Integration tests")]
  public class LoginControllerTester : TesterBase
  {
    public LoginControllerTester(TestServer testServer)
      : base(testServer) { }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ExistingAccountRegisterRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task LoginAsync_Should_ReturnSuccess(RegisterRequest registerRequest)
    {
      var loginRequest = new LoginRequest
      {
        Email = registerRequest.Email,
        Password = registerRequest.Password,
        RememberMe = false
      };

      await LoginAsync(loginRequest);
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ExistingUncinfirmedAccountRegisterRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task LoginAsync_Should_ErrorOut_With_BadRequest(RegisterRequest registerRequest)
    {
      var loginRequest = new LoginRequest
      {
        Email = registerRequest.Email,
        Password = registerRequest.Password,
        RememberMe = false
      };

      var response = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Login), loginRequest, CancellationToken.None);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

      var data = await response.Content.ReadFromJsonAsync<ProblemDetails>();

      data?.Detail.Should().Be(LoginErrors.FailedAttempt.Message);
      data?.Extensions.Should().HaveCount(2).And.ContainKeys("traceId", "requestId");
      data?.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.5.1");
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.NonExistingAccountLoginRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task LoginAsync_Should_ErrorOut_With_NotFound(LoginRequest loginRequest)
    {
      var response = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Login), loginRequest, CancellationToken.None);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);

      var data = await response.Content.ReadFromJsonAsync<ProblemDetails>();

      data?.Detail.Should().Be(LoginErrors.LoginNotFound.Message);
      data?.Extensions.Should().HaveCount(2).And.ContainKeys("traceId", "requestId");
      data?.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.5.5");
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ExistingAccountRegisterRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task RefreshAccessAsync_Should_ReturnSuccess(RegisterRequest registerRequest)
    {
      var loginRequest = new LoginRequest
      {
        Email = registerRequest.Email,
        Password = registerRequest.Password,
        RememberMe = false
      };

      var loginResponse = await LoginAsync(loginRequest);

      var refreshRequest = new RefreshRequest
      {
        AccessToken = loginResponse?.BearerToken!,
        RefreshToken = loginResponse?.RefreshToken!
      };

      var refreshResponse = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Refresh), refreshRequest, CancellationToken.None);

      refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

      var refreshResponseData = await refreshResponse.Content.ReadFromJsonAsync<LoginResponse>();

      refreshResponseData?.BearerToken.Should().NotBe(loginResponse?.BearerToken);
      refreshResponseData?.RefreshToken.Should().Be(loginResponse?.RefreshToken);
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ExistingAccountRefreshRequestInvalidTokens), MemberType = typeof(LoginControllerTestData))]
    public async Task RefreshAccessAsync_Should_ReturnUnauthorized(
      RegisterRequest registerRequest,
      bool invalidBearerToken,
      string? refreshToken)
    {
      var loginRequest = new LoginRequest
      {
        Email = registerRequest.Email,
        Password = registerRequest.Password,
        RememberMe = false
      };

      var loginResponse = await LoginAsync(loginRequest);
      var bearerToken = loginResponse?.BearerToken;

      if (invalidBearerToken)
      {
        var user = new DomitianIDUser
        {
          Email = "fake",
          UserName = "fake"
        };

        var tokenService = base.Services.GetRequiredKeyedService<ITokenService>(AppConstants.InnerKey);
        bearerToken = tokenService.GenerateJwt(user);
      }

      var refreshRequest = new RefreshRequest
      {
        AccessToken = bearerToken!,
        RefreshToken = refreshToken ?? loginResponse?.RefreshToken!
      };

      var refreshResponse = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Refresh), refreshRequest, CancellationToken.None);

      refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

      var refreshResponseData = await refreshResponse.Content.ReadFromJsonAsync<ProblemDetails>();

      refreshResponseData?.Detail.Should().BeNullOrWhiteSpace();
      refreshResponseData?.Extensions.Should().HaveCount(2).And.ContainKeys("traceId", "requestId");
      refreshResponseData?.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.5.2");
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ExistingAccountRegisterRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task RevokeAccessAsync_Should_ReturnSuccess(RegisterRequest registerRequest)
    {
      var loginRequest = new LoginRequest
      {
        Email = registerRequest.Email,
        Password = registerRequest.Password,
        RememberMe = false
      };

      var loginResponse = await LoginAsync(loginRequest);

      var revokeResponse = await base.Client.GetAsync(new Uri($"{ApiUrlPathBuilder.Revoke}/{registerRequest.Email}"));

      revokeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(LoginControllerTestData.ErrorRevokeRequests), MemberType = typeof(LoginControllerTestData))]
    public async Task RevokeAccessAsync_Should_ErrorOut_With_ErrorResponses(
      string? revokeEmail,
      HttpStatusCode httpStatusCode,
      string problemDetailsDetail)
    {
      var revokeResponse = await base.Client.GetAsync(new Uri($"{ApiUrlPathBuilder.Revoke}/{revokeEmail}"));

      revokeResponse.StatusCode.Should().Be(httpStatusCode);

      var revokeResponseData = await revokeResponse.Content.ReadFromJsonAsync<ProblemDetails>();

      revokeResponseData?.Detail.Should().BeNullOrWhiteSpace();
      revokeResponseData?.Extensions.Should().HaveCountGreaterThan(1).And.ContainKeys("traceId", "requestId");
      revokeResponseData?.Type.Should().Be(problemDetailsDetail);
    }

    private async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
      var loginResponse = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Login), loginRequest, CancellationToken.None);

      loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

      var loginResponseData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

      loginResponseData?.UserId.Should().NotBeNullOrWhiteSpace();
      loginResponseData?.BearerToken.Should().NotBeNullOrWhiteSpace();
      loginResponseData?.RefreshToken.Should().NotBeNullOrWhiteSpace();

      return loginResponseData!;
    }
  }
}
