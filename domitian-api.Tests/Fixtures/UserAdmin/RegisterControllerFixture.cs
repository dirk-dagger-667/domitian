using dominitian.Business.Contracts;
using dominitian_api.Helpers;
using dominitian_ui.Infrastructure.Validators;
using domition_api.Controllers.UserAdmin;
using FakeItEasy;

namespace domitian_api.Tests.Fixtures.UserAdmin
{
    public class RegisterControllerFixture
    {
        public RegisterControllerFixture()
        {
            RegisterService = A.Fake<IRegisterService>();
            RegReqValidator = A.Fake<RegisterRequestValidator>();
            ConEmailReqValidator = A.Fake<ConfirmEmailRequestValidator>();
            ReturnResultsHelper = A.Fake<IReturnResultsHelper>();

            SUT = new RegisterController(RegisterService, ReturnResultsHelper);
        }

        public IRegisterService RegisterService { get; }
        public RegisterRequestValidator RegReqValidator { get; }
        public ConfirmEmailRequestValidator ConEmailReqValidator { get; }
        public IReturnResultsHelper ReturnResultsHelper { get; }
        public RegisterController SUT { get; }
    }
}
