using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace domitian.Tests.Integration.DataSources
{
  public class RegisterControllerTestData
  {
    public static IEnumerable<object[]> RegisterAsyncTestData
    {
      get
      {
        // New account
        yield return new object[]
        {
          SeedDataGenerator.GetRegisterRequest(true),
          HttpStatusCode.Created,
          typeof(string),
          null,
          null
        };

        // Existing account
        yield return new object[]
        {
          LoginControllerTestData.ValidAccountSeedData.First(),
          HttpStatusCode.Conflict,
          typeof(ProblemDetails),
          RegisterErrors.RegisterUserExists.Message!,
          "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10"
        };

        // Invalid input request
        var registerRequest = SeedDataGenerator.GetRegisterRequest(true);
        registerRequest.Email = "Invalid Emmail";

        yield return new object[]
        {
          registerRequest,
          HttpStatusCode.BadRequest,
          typeof(ProblemDetails),
          "'Email' is not in the correct format.",
          "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };
      }
    }

    public static IEnumerable<object[]> ConfirmRegistrationAsyncTestData
    {
      get
      {
        yield return new object[]
        {
          LoginControllerTestData.ValidUnconfirmedAccountSeedData.First(),
          HttpStatusCode.OK,
          typeof(string),
          null,
          null
        };

        yield return new object[]
        {
          new RegisterRequest
          {
            Email = "fake@fake.com",
            Password = string.Empty,
            ConfirmPassword = string.Empty,
          },
          HttpStatusCode.Unauthorized,
          typeof(ProblemDetails),
          LoginErrors.LoginNotFound("fake@fake.com").Message!,
          "https://tools.ietf.org/html/rfc9110#section-15.5.2"
        };
      }
    }
  }
}
