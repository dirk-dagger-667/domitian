using domitian.Business.Contracts;
using domitian_api.Controllers.UserAdmin;
using domitian_api.Validators;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace domitian_api.Tests.Fixtures.UserAdmin
{
  public class RegisterControllerFixture
  {
    public RegisterControllerFixture()
    {
      RegisterService = A.Fake<IRegisterService>();
      RegReqValidator = A.Fake<RegisterRequestValidator>();
      ConEmailReqValidator = A.Fake<ConfirmEmailRequestValidator>();
      HttpContext = new DefaultHttpContext();

      SUT = new RegisterController(RegisterService)
      {
        ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
        {
          HttpContext = HttpContext
        }
      };
    }

    public IRegisterService RegisterService { get; }
    public RegisterRequestValidator RegReqValidator { get; }
    public ConfirmEmailRequestValidator ConEmailReqValidator { get; }
    public HttpContext HttpContext { get; }
    public RegisterController SUT { get; }
  }
}
