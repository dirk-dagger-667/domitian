using domitian.Models.Dtos.UserAdmin;
using domitian.Models.Responses.UserAdmin;
using domitian.Models.Results;
using domitian.Models.Results.Errors;
using domitian.Tests.Fixtures.UserAdmin.Users;
using domitian.Tests.Infrastructure.DataSources.UserAdmin.Endpoints;
using domitian.Tests.Infrastructure.Extensions;
using domitian_api.Endpoints;
using domitian_api.Extensions;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace domitian.Tests.UserAdmin.Users
{
  public class GetEndpointsTester(GetEndpointsFixture fixture) : IClassFixture<GetEndpointsFixture>
  {
    [Fact]
    public async Task GetSingleAsync_Success()
    {
      var fakeResult = Result<UserDto>.Success(A.Dummy<UserDto>());

      A.CallTo(() => fixture.UsersService.GetAsync(A<string>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.GetSingleAsync(
        A.Dummy<string>(),
        fixture.UsersService,
        fixture.HttpContext,
        CancellationToken.None);

      TypedResultAssertions.IsOkData(sutResult, fakeResult.Data);
    }

    [Fact]
    public async Task GetSingleAsync_NotFound()
    {
      var fakeResult = Result<UserDto>.Failure("fake error", ResultType.NotFound, Error.Exception);

      A.CallTo(() => fixture.UsersService.GetAsync(A<string>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.GetSingleAsync(
        A.Dummy<string>(),
        fixture.UsersService,
        fixture.HttpContext,
        CancellationToken.None);

      TypedResultAssertions.IsNotFoundData(sutResult,
        fakeResult.ToProblemDetails(fixture.HttpContext, StatusCodes.Status404NotFound));
    }

    [Theory]
    [MemberData(nameof(GetEndpointsTestData.ValidationFailureTestData), MemberType = typeof(GetEndpointsTestData))]
    public async Task ValidatatePaganationParameters_Failure(PaganationParameters paganationParameters)
    {
      A.CallTo(() => fixture.InvocationContext.Arguments)
        .Returns(new List<object?>() { paganationParameters });
      A.CallTo(() => fixture.InvocationContext.GetArgument<PaganationParameters>(0))
        .Returns(paganationParameters);

      var result = await fixture.ValidationFilterUnderTest.InvokeAsync(
        fixture.InvocationContext,
        fixture.Next);

      TypedResultAssertions.IsValidationProblem(result, fixture.HttpContext, fixture.Logger);
    }

    [Fact]
    public async Task GetPageASync_Success()
    {
      var fakeResult = Result<UsersResponse>.Success(A.Dummy<UsersResponse>());

      A.CallTo(() => fixture.UsersService.GetPageAsync(A<PaganationParameters>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.GetPageAsync(
        A.Dummy<PaganationParameters>(),
        fixture.UsersService,
        fixture.HttpContext,
        CancellationToken.None);

      TypedResultAssertions.IsOkData(sutResult, fakeResult.Data);
    }

    [Fact]
    public async Task GetPageAsync_NotFound()
    {
      var fakeResult = Result<UsersResponse>.Failure("fake error", ResultType.NotFound, Error.Exception);

      A.CallTo(() => fixture.UsersService.GetPageAsync(A<PaganationParameters>.Ignored))
        .Returns(fakeResult);

      var sutResult = await UserEndpoints.GetPageAsync(
        A.Dummy<PaganationParameters>(),
        fixture.UsersService,
        fixture.HttpContext,
        CancellationToken.None);

      TypedResultAssertions.IsNotFoundData(sutResult,
        fakeResult.ToProblemDetails(fixture.HttpContext, StatusCodes.Status404NotFound));
    }
  }
}
