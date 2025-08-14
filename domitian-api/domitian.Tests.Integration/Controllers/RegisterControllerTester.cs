using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian.Tests.Integration.DataSources;
using domitian.Tests.Integration.Fixture;
using domitian_api.Data.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;

namespace domitian.Tests.Integration.Controllers
{
  [Collection("Integration tests")]
  public class RegisterControllerTester : TesterBase
  {
    public RegisterControllerTester(TestServer testServer)
      : base(testServer) { }

    [Theory]
    [MemberData(nameof(RegisterControllerTestData.RegisterAsyncTestData), MemberType = typeof(RegisterControllerTestData))]
    public async Task RegisterAsync_Should_ReturnResponses(
      RegisterRequest registerRequest,
      HttpStatusCode httpStatusCode,
      Type responseType,
      string? errorMessage,
      string? type)
    {
      var registerResponse = await base.Client.PostAsJsonAsync(new Uri(ApiUrlPathBuilder.Register), registerRequest, CancellationToken.None);

      registerResponse.StatusCode.Should().Be(httpStatusCode);

      var registerResponseData = responseType == typeof(string)
        ? await registerResponse.Content.ReadAsStringAsync()
        : await registerResponse.Content.ReadFromJsonAsync(responseType);

      if (responseType == typeof(string))
      {
        registerResponseData?.ToString().Should().NotBeNullOrWhiteSpace();
      }
      else
      {
        var problemDetails = (ProblemDetails)registerResponseData!;
        problemDetails?.Extensions.Should().HaveCountGreaterThan(1).And.ContainKeys("traceId", "requestId");
        problemDetails?.Extensions["traceId"]?.ToString().Should().NotBeNullOrWhiteSpace();
        problemDetails?.Extensions["requestId"]?.ToString().Should().NotBeNullOrWhiteSpace();
        problemDetails?.Detail.Should().Contain(errorMessage);
        problemDetails?.Type.Should().Be(type);
      }
    }

    [Fact]
    public async Task ConfirmEmailAsync_Should_ReturnsSuccess()
    {
      var userEmail = LoginControllerTestData.ValidUnconfirmedAccountSeedData.First().Email;
      var userManager = base.Services.GetRequiredService<UserManager<DomitianIDUser>>();
      var user = userManager.Users
        .Where(user => user.Email == userEmail)
        .First();

      var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

      var confirmEmailResponse = await base.Client
        .GetAsync(new Uri($"{ApiUrlPathBuilder.ConfirmEmail}/{user.Id}/{code}"));

      confirmEmailResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ConfirmEmailAsync_Should_ReturnsFailure()
    {
      var registerResponse = await base.Client.GetAsync(new Uri($"{ApiUrlPathBuilder.ConfirmEmail}/fakeUserId/fakeCode"));

      registerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

      var registerResponseData = await registerResponse.Content.ReadFromJsonAsync<ProblemDetails>();

      registerResponseData?.Extensions.Should().HaveCountGreaterThan(1).And.ContainKeys("traceId", "requestId");
      registerResponseData?.Extensions["traceId"]?.ToString().Should().NotBeNullOrWhiteSpace();
      registerResponseData?.Extensions["requestId"]?.ToString().Should().NotBeNullOrWhiteSpace();
      registerResponseData?.Detail.Should().Be(LoginErrors.LoginNotFound.Message);
      registerResponseData?.Type.Should().Be("https://tools.ietf.org/html/rfc9110#section-15.5.5");
    }

    [Theory]
    [MemberData(nameof(RegisterControllerTestData.ConfirmRegistrationAsyncTestData), MemberType = typeof(RegisterControllerTestData))]
    public async Task ConfirmRegistrationAsync_Should_ReturnResponses(
      RegisterRequest registerRequest,
      HttpStatusCode httpStatusCode,
      Type responseType,
      string? errorMessage,
      string? type)
    {
      var registerResponse = await base.Client.GetAsync(new Uri($"{ApiUrlPathBuilder.ConfirmRegistration}/{registerRequest.Email}"));

      registerResponse.StatusCode.Should().Be(httpStatusCode);

      var registerResponseData = responseType == typeof(string)
        ? await registerResponse.Content.ReadAsStringAsync()
        : await registerResponse.Content.ReadFromJsonAsync(responseType);

      if (responseType == typeof(string))
      {
        registerResponseData?.ToString().Should().NotBeNullOrWhiteSpace();
      }
      else
      {
        var problemDetails = (ProblemDetails)registerResponseData!;
        problemDetails?.Extensions.Should().HaveCountGreaterThan(1).And.ContainKeys("traceId", "requestId");
        problemDetails?.Extensions["traceId"]?.ToString().Should().NotBeNullOrWhiteSpace();
        problemDetails?.Extensions["requestId"]?.ToString().Should().NotBeNullOrWhiteSpace();
        problemDetails?.Detail.Should().Be(errorMessage);
        problemDetails?.Type.Should().Be(type);
      }
    }
  }
}
