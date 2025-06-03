using domitian.Models.Extensions;
using domitian.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace domitian_api.Helpers
{
  public class ReturnResultsHelper(IHttpContextAccessor _httpContextAccessor)
      : IReturnResultsHelper
  {
    public IActionResult ResultTypeToActionResult<T>(Result<T> result)
    {
      if (result.Type == ResultType.CreatedAt)
      {
        return new CreatedResult(GetUri(), result.Data);
      }

      if (result.Type == ResultType.Ok)
      {
        return new OkObjectResult(result.Data);
      }

      return ResultTypeToActionResultBase(result);
    }

    public IActionResult ResultTypeToActionResultBase(Result result)
    {
      switch (result.Type)
      {
        case ResultType.Ok:
          return new OkResult();
        case ResultType.BadRequest:
          return new BadRequestObjectResult(BuildProblemDetails(StatusCodes.Status400BadRequest));
        case ResultType.CreatedAt:
          return new CreatedResult(GetUri(), null);
        case ResultType.NotFound:
          return new NotFoundObjectResult(BuildProblemDetails(StatusCodes.Status404NotFound));
        case ResultType.Unauthorized:
          return new UnauthorizedResult();
        case ResultType.Conflict:
          return new ConflictObjectResult(BuildProblemDetails(StatusCodes.Status409Conflict));
        default:
          return new BadRequestObjectResult(BuildProblemDetails(StatusCodes.Status400BadRequest));
      }

      ProblemDetails BuildProblemDetails(int statusCode)
        => new ProblemDetails()
          .WithType(SelectResultType(result.Type))
          .WithTitle(result.Title)
          .WithInstance(_httpContextAccessor.HttpContext)
          .WithStatus(statusCode)
          .WithDetail(result.Error?.Message)
          .WithRequestId(_httpContextAccessor.HttpContext)
          .WithTraceId(_httpContextAccessor.HttpContext);
    }

    private string SelectResultType(ResultType type) => type switch
    {
      ResultType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
      ResultType.Unauthorized => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
      ResultType.BadRequest => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
      ResultType.Conflict => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
      _ => "https://tools.ietf.org/html/rfc9110#section-15.5.1"
    };

    private string GetUri()
    {
      var httpRequest = _httpContextAccessor.HttpContext?.Request;
      var uri = $"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.Path}{httpRequest?.QueryString}";

      return uri;
    }
  }
}
