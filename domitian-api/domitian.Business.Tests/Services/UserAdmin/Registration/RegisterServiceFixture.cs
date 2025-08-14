using domitian.Business.Contracts;
using domitian.Business.Services.RegisterService;
using domitian.Infrastructure.Configuration.Authentication;
using domitian_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace domitian.Business.Tests.Services.UserAdmin.Registration
{
    public class RegisterServiceFixture : IDisposable
    {
        public RegisterServiceFixture()
        {
            UserManager = A.Fake<UserManager<DomitianIDUser>>();
            UserStore = A.Fake<IUserStore<DomitianIDUser>>(op =>
                op.Implements<IUserEmailStore<DomitianIDUser>>());
            SignInManager = A.Fake<SignInManager<DomitianIDUser>>();
            EmailSender = A.Fake<IEmailSender>();
            Logger = A.Fake<ILogger<RegisterService>>();
            UrlOptions = A.Fake<IOptionsMonitor<ApiUrlOptions>>();

            SUT = new RegisterService(UserManager, UserStore, SignInManager, EmailSender, Logger, UrlOptions);
        }

        public UserManager<DomitianIDUser> UserManager { get; }
        public IUserStore<DomitianIDUser> UserStore { get; }
        public SignInManager<DomitianIDUser> SignInManager { get; }
        public IEmailSender EmailSender { get; }
        public ILogger<RegisterService> Logger { get; }
        public IOptionsMonitor<ApiUrlOptions> UrlOptions { get; }
        public IRegisterService SUT { get; }

        public void Dispose()
        {
            UserManager.Dispose();
            UserStore.Dispose();
        }
    }
}
