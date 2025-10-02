using domitian.Models.Requests.Users;
using domitian.Models.Results;
using domitian.Models.Results.Errors;
using domitian.Tests.Fixtures.UserAdmin.Users;
using domitian.Tests.Infrastructure.DataSources.UserAdmin.Endpoints;
using domitian.Tests.Infrastructure.Extensions;
using domitian_api.Endpoints;
using domitian_api.Extensions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace domitian.Tests.UserAdmin.Users
{
  public class PutEndpointsTester(PutEndpointsFixture fixture) : IClassFixture<PutEndpointsFixture>
  {
    [Fact]
    public async Task PutSingle_Success()
    {
      var fakeResult = Result.NoContent();

      A.CallTo(() => fixture.UsersService.UpdateAsync(A<string>.Ignored, A<UpdateUserRequest>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.PutSingleAsync(
        A.Dummy<string>(),
        A.Dummy<UpdateUserRequest>(),
        fixture.UsersService,
        fixture.HttpContext);

      sutResult.Should().BeAssignableTo<NoContent>();
    }

    [Fact]
    public async Task GetSingleAsync_NotFound()
    {
      var fakeResult = Result.Failure("fake error", ResultType.NotFound, UsersErrors.UsersNotFound);

      A.CallTo(() => fixture.UsersService.UpdateAsync(A<string>.Ignored, A<UpdateUserRequest>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.PutSingleAsync(
        A.Dummy<string>(),
        A.Dummy<UpdateUserRequest>(),
        fixture.UsersService,
        fixture.HttpContext);

      TypedResultAssertions.IsNotFoundData(sutResult,
        fakeResult.ToProblemDetails(fixture.HttpContext, StatusCodes.Status404NotFound));
    }

    [Theory]
    [MemberData(nameof(PutEndpointsTestData.ValidationFailureSingleTestData), MemberType = typeof(PutEndpointsTestData))]
    public async Task ValidateUpdateUserRequest_Single_Failure(UpdateUserRequest request)
    {
      A.CallTo(() => fixture.InvocationContext.Arguments)
        .Returns(new List<object?>() { request });
      A.CallTo(() => fixture.InvocationContext.GetArgument<UpdateUserRequest>(0))
        .Returns(request);

      var result = await fixture.ValidationFilterSingleUnderTest.InvokeAsync(
        fixture.InvocationContext,
        fixture.Next);

      TypedResultAssertions.IsValidationProblem(result,
        fixture.HttpContext,
        fixture.SingleLogger);
    }

    [Fact]
    public async Task ValidateUpdateUsersRequest_Bulk_Failure_Null_Arguments()
    {
      A.CallTo(() => fixture.InvocationContext.Arguments)
        .Returns(new List<object?>() { null });
      A.CallTo(() => fixture.InvocationContext.GetArgument<IEnumerable<UpdateUserRequest>?>(0))
        .Returns(null);

      var result = await fixture.ValidationFilterSingleUnderTest.InvokeAsync(
        fixture.InvocationContext,
        fixture.Next);

      TypedResultAssertions.IsValidationProblem(result,
        fixture.HttpContext
        , fixture.BulkLogger);
    }

    [Theory]
    [MemberData(nameof(PutEndpointsTestData.ValidationFailureBulkTestData), MemberType = typeof(PutEndpointsTestData))]
    public async Task ValidateUpdateUsersRequest_Bulk_Failure(IEnumerable<UpdateUserRequest> requests)
    {
      A.CallTo(() => fixture.InvocationContext.Arguments)
        .Returns(new List<object?>() { requests });
      A.CallTo(() => fixture.InvocationContext.GetArgument<IEnumerable<UpdateUserRequest>>(0))
        .Returns(requests);

      var result = await fixture.ValidationFilterBulkUnderTest.InvokeAsync(
        fixture.InvocationContext,
        fixture.Next);

      TypedResultAssertions.IsValidationProblem(result,
        fixture.HttpContext,
        fixture.BulkLogger);
    }
  }
}

