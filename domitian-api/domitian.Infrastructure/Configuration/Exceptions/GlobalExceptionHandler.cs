//using domitian.Infrastructure.Configuration.Formatters;
//using domitian.Models.Extensions;
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using Microsoft.Extensions.DependencyInjection;

//namespace domitian.Infrastructure.Configuration.Exceptions
//{
//  public class GlobalExceptionHandler(
//    IProblemDetailsService problemDetailsService)
//    : IExceptionHandler
//  {
//    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//    {
//      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

//      var contex = new ProblemDetailsContext
//      {
//        HttpContext = httpContext,
//        Exception = exception,
//        ProblemDetails =
//        {
//          Type = exception.GetType().Name,
//          Title = exception.GetInnermostExceptionMessage(),
//          Detail = exception.GetInnermostExceptionMessage()
//        }
//      };

//      var acceptHeader = httpContext.Request.Headers["Accept"].Any() ? httpContext.Request.Headers["Accept"].FirstOrDefault() : null;

//      if (!string.IsNullOrWhiteSpace(acceptHeader)
//        && acceptHeader.Contains("/xml")
//        || acceptHeader.Contains("+xml"))
//      {
//        var body = "asdasadsadsdsadasdsadsadsadsa";

//        //await httpContext.Response.WriteAsync(body);

//        var result = new ObjectResult(body)
//        {
//          StatusCode = httpContext.Response.StatusCode,
//          ContentTypes = { "application/problem+xml", "application/xml" }
//        };

//        var formatter = httpContext.RequestServices.GetRequiredService<IRegisterService>();

//        result.Formatters.Add(formatter);

//        await result.ExecuteResultAsync(new ActionContext { HttpContext = httpContext });

//        return true;
//      }

//      return await problemDetailsService.TryWriteAsync(contex);
//    }
//  }
//}
