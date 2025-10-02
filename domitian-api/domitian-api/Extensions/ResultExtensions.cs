using domitian.Models.Extensions;
using domitian.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace domitian_api.Extensions
{
  public static class ResultExtensions
  {
    public static IActionResult ToActionResultWithData<T>(this Result<T> result, HttpContext httpContext)
    {
      if (result.Type == ResultType.CreatedAt)
        return new CreatedResult(httpContext.Request.ToUriString(), result.Data);

      if (result.Type == ResultType.Ok)
        return new OkObjectResult(result.Data);

      return result.ToActionResult(httpContext);
    }

    public static IActionResult ToActionResult(this Result result, HttpContext httpContext)
    => result.Type switch
    {
      ResultType.Ok => new OkResult(),
      ResultType.BadRequest => new BadRequestObjectResult(result.ToProblemDetails(httpContext, StatusCodes.Status400BadRequest)),
      ResultType.CreatedAt => new CreatedResult(httpContext.Request.ToUriString(), null),
      ResultType.NotFound => new NotFoundObjectResult(result.ToProblemDetails(httpContext, StatusCodes.Status404NotFound)),
      ResultType.Unauthorized => new UnauthorizedObjectResult(result.ToProblemDetails(httpContext, StatusCodes.Status401Unauthorized)),
      ResultType.Conflict => new ConflictObjectResult(result.ToProblemDetails(httpContext, StatusCodes.Status409Conflict)),
      _ => new BadRequestObjectResult(result.ToProblemDetails(httpContext,StatusCodes.Status400BadRequest))
    };

    public static IResult ToTypedResultWithData<T>(this Result<T> result, HttpContext httpContext)
    {
      var routeName = httpContext
          .GetEndpoint()?
          .Metadata?
          .GetMetadata<RouteNameMetadata>()?
          .RouteName;

      var routeValues = httpContext?.Request.RouteValues;

      if (result.Type == ResultType.CreatedAt)
        return TypedResults.CreatedAtRoute<T>(result.Data, routeName, routeValues);

      if (result.Type == ResultType.Ok)
        return TypedResults.Ok(result.Data);

      return result.ToTypedResult(httpContext!);
    }

    public static IResult ToTypedResult(this Result result, HttpContext httpContext)
    {
      var routeName = httpContext?
          .GetEndpoint()?
          .Metadata?
          .GetMetadata<RouteNameMetadata>()?
          .RouteName;

      var routeValues = httpContext?.Request.RouteValues;

      return result.Type switch
      {
        ResultType.Ok => TypedResults.Ok(),
        ResultType.NoContent => TypedResults.NoContent(),
        ResultType.BadRequest => TypedResults.BadRequest(result.ToProblemDetails(httpContext!, StatusCodes.Status400BadRequest)),
        ResultType.CreatedAt => TypedResults.CreatedAtRoute(routeName, routeValues),
        ResultType.NotFound => TypedResults.NotFound(result.ToProblemDetails(httpContext!, StatusCodes.Status404NotFound)),
        ResultType.Unauthorized => TypedResults.Unauthorized(),
        ResultType.Conflict => TypedResults.Conflict(result.ToProblemDetails(httpContext!, StatusCodes.Status409Conflict)),
        _ => TypedResults.BadRequest(result.ToProblemDetails(httpContext!, StatusCodes.Status400BadRequest))
      };
    }

    public static ProblemDetails ToProblemDetails(this Result result,
      HttpContext httpContext,
      int statusCode)
      => new ProblemDetails()
          .WithType(result.ToRfcReference())
          .WithTitle(result.Title)
          .WithInstance(httpContext)
          .WithStatus(statusCode)
          .WithDetail(result.Error?.Message)
          .WithRequestId(httpContext)
          .WithTraceId(httpContext);


    private static string ToRfcReference(this Result result) => result.Type switch
    {
      ResultType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
      ResultType.Unauthorized => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
      ResultType.BadRequest => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
      ResultType.Conflict => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
      _ => "https://tools.ietf.org/html/rfc9110#section-15.5.1"
    };
  }
}
