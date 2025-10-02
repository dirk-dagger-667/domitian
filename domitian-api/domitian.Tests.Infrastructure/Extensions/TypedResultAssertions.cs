using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace domitian.Tests.Infrastructure.Extensions
{
  public static class TypedResultAssertions
  {
    public static void IsOkData<T>(IResult result, T data)
    {
      var ok = result.As<Ok<T>>();
      ok.Should().NotBeNull();
      ok.Value.Should().NotBeNull().And.Be(data);
    }

    public static void IsNotFoundData(IResult result, ProblemDetails pd)
    {
      var ok = result.As<NotFound<ProblemDetails>>();
      ok.Should().NotBeNull();
      ok.Value.Should().NotBeNull();
      ok.Value.Should().BeEquivalentTo(pd);
    }

    public static void IsBadRequest(IResult result)
    {
      var ok = result.As<BadRequest<ProblemDetails>>();
      ok.Should().NotBeNull();
      ok.Value.Should().NotBeNull();
      ok.Value.Status.Should().Be(StatusCodes.Status400BadRequest);
    }

    public static void IsInternalServerError(IResult result)
    {
      var ok = result.As<InternalServerError<ProblemDetails>>();
      ok.Should().NotBeNull();
      ok.Value.Should().NotBeNull();
      ok.Value.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }

    public static void IsValidationProblem<T>(object? result, HttpContext httpContext, ILogger<T> logger)
    {
      result.Should().NotBeNull().And.BeOfType<ValidationProblem>();

      var vp = result as ValidationProblem;
      vp?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
      vp?.ProblemDetails.Should().NotBeNull();
      vp?.ProblemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
      vp?.ProblemDetails.Errors.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
      vp?.ProblemDetails.Detail.Should().NotBeNull();
      vp?.ProblemDetails.Title.Should().NotBeNull().And.Be("One or more validation errors occurred.");
      vp?.ProblemDetails.Instance.Should().NotBeEmpty().And.Be($"{httpContext?.Request.Method} {httpContext?.Request.Path}");
      vp?.ProblemDetails.Type.Should().NotBeNull().And.Be("https://tools.ietf.org/html/rfc9110#section-15.5.1");

      Fake.GetCalls(logger).ToList().Should().HaveCountGreaterThanOrEqualTo(1);
      Fake.GetCalls(logger).ToList().First().Method.Name.Should().Be("Log");
    }
  }
}
