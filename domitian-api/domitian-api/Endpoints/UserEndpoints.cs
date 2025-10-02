using domitian.Business.Contracts;
using domitian.Models.Dtos.UserAdmin;
using domitian.Models.Requests.Users;
using domitian.Models.Responses.UserAdmin;
using domitian_api.Configuraion;
using domitian_api.Extensions;
using domitian_api.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;

namespace domitian_api.Endpoints
{
  public static class UserEndpoints
  {
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
      var users = app.MapGroup("api/users")
        .WithTags("Users");

      users.MapGet("/", GetPageAsync)
        .AddEndpointFilter<ValidationFilter<PaganationParameters>>()
        .Produces<UsersResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

      users.MapGet("/{id}", GetSingleAsync)
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

      users.MapPut("/{id}", PutSingleAsync)
        .AddEndpointFilter<ValidationFilter<UpdateUserRequest>>()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

      users.MapPut("/", PutBulkAsync)
        .AddEndpointFilter<ValidationFilter<IEnumerable<UpdateUserRequest>>>()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

      users.MapDelete("/{id}", DeleteSingleAsync)
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

      users.MapDelete("/", DeleteBulkAsync)
        .AddEndpointFilter<ValidationFilter<IEnumerable<string>>>()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> GetSingleAsync(
      [FromRoute] string id,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext,
      CancellationToken cancellationToken)
      => (await usersService.GetAsync(id)).ToTypedResultWithData(httpContext);

    public static async Task<IResult> GetPageAsync(
      [AsParameters] PaganationParameters pageParams,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext,
      CancellationToken cancellationToken)
      => (await usersService.GetPageAsync(pageParams)).ToTypedResultWithData(httpContext);

    public static async Task<IResult> PutSingleAsync(
      [FromRoute] string id,
      [FromBody] UpdateUserRequest updateUserRequest,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext)
      => (await usersService.UpdateAsync(id, updateUserRequest)).ToTypedResult(httpContext);

    public static async Task<IResult> PutBulkAsync(
      [FromBody] IEnumerable<UpdateUserRequest> updateUsersRequest,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext,
      CancellationToken cancellationToken)
      => (await usersService.UpdateBulkAsync(updateUsersRequest)).ToTypedResult(httpContext);

    public static async Task<IResult> DeleteSingleAsync(
      [FromRoute] string id,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext)
      => (await usersService.DeleteAsync(id)).ToTypedResult(httpContext);

    public static async Task<IResult> DeleteBulkAsync(
      [FromBody] IEnumerable<string> ids,
      [FromKeyedServices(AppConstants.CrossCuttingKey)] IUsersService usersService,
      HttpContext httpContext)
    {
      var result = await usersService.DeleteBulkAsync(ids);

      return result.ToTypedResult(httpContext);
    }
  }
}
