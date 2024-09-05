using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace dominitian.Tests.Infrastructure.Extensions
{
    public class ActionResultAssertions
    {
        public static void IsBadRequest(IActionResult result, string errorMessage)
        {

            var badRequest = IsOfHttpResultType<BadRequestObjectResult>(result, HttpStatusCode.BadRequest);

            badRequest?.Value.Should().NotBeNull()
                .And.BeOfType<string>();

            var data = badRequest?.Value?.As<string>();

            data.As<string>().Should().NotBeNull()
                .And.Contain(errorMessage);
        }

        public static void IsValidationProblem(IActionResult result, string errorMessage)
        {
            result.Should().NotBeNull().And.BeAssignableTo<IStatusCodeActionResult>();
            var valProb = result.As<ObjectResult>();

            valProb?.Value.Should().NotBeNull()
                .And.BeOfType<ValidationProblemDetails>();

            var data = valProb?.Value?.As<ValidationProblemDetails>();

            data?.Detail.Should().NotBeNull()
                .And.Contain(errorMessage);
        }

        public static void IsNotFound(IActionResult result, string errorMessage)
        {
            var notFound = IsOfHttpResultType<NotFoundObjectResult>(result, HttpStatusCode.NotFound);

            notFound?.Value.Should().NotBeNull()
                .And.BeOfType<string>();

            var data = notFound?.Value?.As<string>();

            data.Should().NotBeNull()
                .And.Contain(errorMessage);
        }

        public static void IsCreated(IActionResult result)
            => IsOfHttpResultType<CreatedResult>(result, HttpStatusCode.Created);

        public static void IsCreated<T>(IActionResult result, T value)
        {
            var createdRes = IsOfHttpResultType<CreatedResult>(result, HttpStatusCode.Created);

            createdRes?.Value.Should().NotBeNull()
                .And.BeOfType<T>()
                .And.Be(value);
        }

        public static void IsOk(IActionResult result)
            => IsOfHttpResultType<OkObjectResult>(result, HttpStatusCode.OK);

        public static void IsOk<T>(IActionResult result, T expectedResult)
        {
            var okRes = IsOfHttpResultType<OkObjectResult>(result, HttpStatusCode.OK);

            okRes?.Value.Should().NotBeNull()
                .And.BeAssignableTo<T>()
                .And.Be(expectedResult);
        }

        public static void IsUnauthorized(IActionResult result)
            => IsOfHttpResultType<UnauthorizedResult>(result, HttpStatusCode.Unauthorized);

        private static T IsOfHttpResultType<T>(IActionResult result, HttpStatusCode statusCode)
            where T : IStatusCodeActionResult
        {
            result.Should().NotBeNull().And.BeAssignableTo<T>();

            var httpResult = (T)result;

            httpResult?.StatusCode.Should().Be((int)statusCode);

            return httpResult!;
        }
    }
}
