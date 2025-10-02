using domitian.Business.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;

namespace domitian.Tests.Fixtures.UserAdmin.Users
{
  public abstract class UserEndpointsBase
  {
    public UserEndpointsBase()
    {
      UsersService = A.Fake<IUsersService>();

      HttpContext = new DefaultHttpContext();
      HttpContext.Request.Method = "fake method";
      HttpContext.Request.Path = "/fake path";
      HttpContext.TraceIdentifier = "fake request id";
      HttpContext.Features.Set(A.Dummy<IHttpActivityFeature>().Activity = new Activity("fake activity"));

      Next = A.Fake<EndpointFilterDelegate>();
      InvocationContext = A.Fake<EndpointFilterInvocationContext>();

      A.CallTo(() => InvocationContext.HttpContext).Returns(HttpContext);
    }

    public IUsersService UsersService { get; set; }
    public HttpContext HttpContext { get; set; }
    public EndpointFilterInvocationContext InvocationContext { get; set; }
    public EndpointFilterDelegate Next { get; set; }
  }
}
