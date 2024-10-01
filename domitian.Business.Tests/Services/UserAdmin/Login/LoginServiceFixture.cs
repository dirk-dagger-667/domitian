using domitian.Business.Contracts;
using domitian.Business.Services;
using domitian_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Tests.Services.UserAdmin.Login
{
    public class LoginServiceFixture
    {
        public LoginServiceFixture()
        {
            SignInManager = A.Fake<SignInManager<DomitianIDUser>>();
            Logger = A.Fake<ILogger<LoginService>>();
            HtppContext = A.Fake<HttpContext>();
            TokenService = A.Fake<ITokenService>();
            AuthenticationService = A.Fake<IAuthenticationService>();

            SUT = new LoginService(SignInManager, TokenService, Logger);
        }

        public ILoginService SUT { get; }
        public ILogger<LoginService> Logger { get; }
        public HttpContext HtppContext { get; }
        public ITokenService TokenService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public SignInManager<DomitianIDUser> SignInManager { get; }
    }
}
