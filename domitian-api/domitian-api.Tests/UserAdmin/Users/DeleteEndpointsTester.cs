using domitian.Models.Results;
using domitian.Models.Results.Errors;
using domitian.Tests.Fixtures.UserAdmin.Users;
using domitian.Tests.Infrastructure.Extensions;
using domitian_api.Endpoints;
using domitian_api.Extensions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace domitian.Tests.UserAdmin.Users
{
  public class DeleteEndpointsTester(DeleteEndpointsFixture fixture)
    : IClassFixture<DeleteEndpointsFixture>
  {
    [Fact]
    public async Task DeleteSingleAsync_Success()
    {
      var fakeResult = Result.NoContent();

      A.CallTo(() => fixture.UsersService.DeleteAsync(A<string>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.DeleteSingleAsync(
        A.Dummy<string>(),
        fixture.UsersService,
        fixture.HttpContext);

      sutResult.Should().BeAssignableTo<NoContent>();
    }

    [Fact]
    public async Task DeleteSingleAsync_Failure()
    {
      var fakeResult = Result.Failure("fake error", ResultType.NotFound, Error.Exception);

      A.CallTo(() => fixture.UsersService.DeleteAsync(A<string>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.DeleteSingleAsync(
        A.Dummy<string>(),
        fixture.UsersService,
        fixture.HttpContext);

      TypedResultAssertions.IsNotFoundData(sutResult,
        fakeResult.ToProblemDetails(fixture.HttpContext, StatusCodes.Status404NotFound));
    }

    [Fact]
    public async Task ValidatateIds_Failure()
    {
      A.CallTo(() => fixture.InvocationContext.Arguments)
        .Returns(new List<object?>() { null });
      A.CallTo(() => fixture.InvocationContext.GetArgument<IEnumerable<string>>(0))
        .Returns(null);

      var result = await fixture.IdsValidator.InvokeAsync(
        fixture.InvocationContext,
        fixture.Next);

      TypedResultAssertions.IsValidationProblem(result, fixture.HttpContext, fixture.Logger);
    }

    [Fact]
    public async Task DeleteBulkAsync_Success()
    {
      var fakeResult = Result.NoContent();

      A.CallTo(() => fixture.UsersService.DeleteBulkAsync(A<IEnumerable<string>>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.DeleteBulkAsync(
        A.Dummy<IEnumerable<string>>(),
        fixture.UsersService,
        fixture.HttpContext);

      sutResult.Should().BeAssignableTo<NoContent>();
    }

    [Fact]
    public async Task DeleteBulkAsync_NotFound()
    {
      var fakeResult = Result.Failure("fake error", ResultType.NotFound, Error.Exception);

      A.CallTo(() => fixture.UsersService.DeleteBulkAsync(A<IEnumerable<string>>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.DeleteBulkAsync(
        A.Dummy<IEnumerable<string>>(),
        fixture.UsersService,
        fixture.HttpContext);

      TypedResultAssertions.IsNotFoundData(sutResult,
        fakeResult.ToProblemDetails(fixture.HttpContext, StatusCodes.Status404NotFound));
    }
  }
}
