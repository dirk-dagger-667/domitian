using dominitian.Business.Contracts;
using dominitian.Business.Services;
using dominitian.Infrastructure.Configuration;
using domition_api.Data.Identity;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dominitian.Business.Tests.Services.UserAdmin.Registration
{
    public class RegisterServiceFixture : IDisposable
    {
        public RegisterServiceFixture()
        {
            UserManager = A.Fake<UserManager<DominitianIDUser>>();
            UserStore = A.Fake<IUserStore<DominitianIDUser>>(op =>
                op.Implements<IUserEmailStore<DominitianIDUser>>());
            SignInManager = A.Fake<SignInManager<DominitianIDUser>>();
            EmailSender = A.Fake<IEmailSender>();
            Logger = A.Fake<ILogger<RegisterService>>();
            UrlOptions = A.Fake<IOptionsMonitor<ApiUrlOptions>>();

            SUT = new RegisterService(UserManager, UserStore, SignInManager, EmailSender, Logger, UrlOptions);
        }

        public UserManager<DominitianIDUser> UserManager { get; }
        public IUserStore<DominitianIDUser> UserStore { get; }
        public SignInManager<DominitianIDUser> SignInManager { get; }
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
