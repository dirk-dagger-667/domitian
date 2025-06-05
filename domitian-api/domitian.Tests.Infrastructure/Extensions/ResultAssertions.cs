using domitian.Models.Results;
using FluentAssertions;

namespace domitian.Tests.Infrastructure.Extensions
{
  public class ResultAssertions
  {
    public static void IsOk(Result result) => IsSuccess(result, ResultType.Ok);

    public static void IsOkData<T>(Result<T> result)
    {
      IsOk(result);
      result.As<Result<T>>().Should().NotBeNull();
      result.Data.Should().NotBeNull().And.BeAssignableTo<T>();
    }

    public static void IsCreated(Result result) => IsSuccess(result, ResultType.CreatedAt);

    public static void IsUnauthorized(Result result) => IsFailure(result, ResultType.Unauthorized);

    public static void IsConflict(Result result, Error error) => IsFailureWithMessage(result, ResultType.Conflict, error);

    public static void IsNotFound(Result result, Error error)
        => IsFailureWithMessage(result, ResultType.NotFound, error);

    public static void IsBadRequest(Result result, Error error)
        => IsFailureWithMessage(result, ResultType.BadRequest, error);

    public static void IsException(Result result)
    {
      IsBadRequest(result, Error.Exception);
      result.Exception.Should().NotBeNull();
      result.InnerException.Should().NotBeNullOrWhiteSpace();
    }

    private static void IsFailureWithMessage(Result result, ResultType type, Error error)
    {
      IsFailure(result, type);
      result.Error?.Code.Should().Be(error.Code);
      result.Error?.Message.Should().NotBeNullOrWhiteSpace().And.Contain(error.Message);
    }

    private static void ResultIs(Result result, ResultType type)
    {
      result.Should().NotBeNull();
      result.Type.Should().Be(type);
    }

    private static void IsFailure(Result result, ResultType type)
    {
      ResultIs(result, type);
      result.IsFailure.Should().BeTrue();
    }

    private static void IsSuccess(Result result, ResultType type)
    {
      ResultIs(result, type);
      result.Error?.Code.Should().Be(ErrorCodes.NoError);
      result.Error?.Message.Should().BeNullOrWhiteSpace();
    }
  }
}
