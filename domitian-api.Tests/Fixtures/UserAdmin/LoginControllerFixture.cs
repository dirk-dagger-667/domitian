using dominitian.Business.Contracts;
using dominitian.Infrastructure.Validators;
using dominitian_api.Helpers;
using domition_api.Controllers.UserAdmin;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace domitian_api.Tests.Fixtures.UserAdmin
{
    public class LoginControllerFixture
    {
        public LoginControllerFixture()
        {
            LoginService = A.Fake<ILoginService>();
            HttpContext = A.Fake<HttpContext>();
            LogResVal = A.Fake<LoginRequestValidator>();
            ReturnResultsHelper = A.Fake<IReturnResultsHelper>();
            RefReqVal = A.Fake<RefreshRequestValidator>();

            SUT = new LoginController(LoginService, ReturnResultsHelper);
        }

        public ILoginService LoginService { get; }
        public HttpContext HttpContext { get; }
        public LoginRequestValidator LogResVal { get; }
        public RefreshRequestValidator RefReqVal { get; }
        public IReturnResultsHelper ReturnResultsHelper { get; }
        public LoginController SUT { get; }
    }
}
