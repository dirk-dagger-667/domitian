using dominitian_ui.Models.Results;
using FluentAssertions;

namespace dominitian.Tests.Infrastructure.Extensions
{
    public class ResultAssertions
    {
        public static void IsOk(Result result) => IsSuccess(result, ResultTypes.Ok);

        public static void IsOkData<T>(Result<T> result)
        {
            IsOk(result);
            result.As<Result<T>>().Should().NotBeNull();
            result.Data.Should().NotBeNull().And.BeAssignableTo<T>();
        }

        public static void IsCreated(Result result) => IsSuccess(result, ResultTypes.CreatedAt);

        public static void IsUnauthorized(Result result) => IsFailure(result, ResultTypes.Unauthorized);

        public static void NotFound(Result result, Error error)
            => IsFailureWithMessage(result, ResultTypes.NotFound, error);

        public static void IsBadRequest(Result result, Error error)
            => IsFailureWithMessage(result, ResultTypes.BadRequest, error);

        public static void IsException(Result result)
        {
            IsBadRequest(result, Error.Exception);
            result.Exception.Should().NotBeNull();
            result.InnerException.Should().NotBeNullOrWhiteSpace();
        }

        private static void IsFailureWithMessage(Result result, ResultTypes type, Error error)
        {
            IsFailure(result, type);
            result.Error?.Code.Should().Be(error.Code);
            result.Error?.Message.Should().NotBeNullOrWhiteSpace().And.Contain(error.Message);
        }

        private static void ResultIs(Result result, ResultTypes type)
        {
            result.Should().NotBeNull();
            result.Type.Should().Be(type);
        }

        private static void IsFailure(Result result, ResultTypes type)
        {
            ResultIs(result, type);
            result.IsFailure.Should().BeTrue();
        }

        private static void IsSuccess(Result result, ResultTypes type)
        {
            ResultIs(result, type);
            result.Error?.Code.Should().Be(ErrorCodes.NoError);
            result.Error?.Message.Should().BeNullOrWhiteSpace();
        }
    }
}
