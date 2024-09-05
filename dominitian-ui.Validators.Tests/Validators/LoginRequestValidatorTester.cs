using DeepCopy;
using dominitian.Infrastructure.Validators;
using dominitian_ui.Infrastructure.Tests.DataSources;
using dominitian_ui.Models.Requests.Login;
using FluentValidation.TestHelper;

namespace dominitian_ui.Infrastructure.Tests.Validators
{
    public class LoginRequestValidatorTester
    {
        private readonly LoginRequestValidator _validator;
        private readonly LoginRequest _validRequest;

        public LoginRequestValidatorTester()
        {
            _validator = new LoginRequestValidator();
            _validRequest = new LoginRequest()
            { 
                Email = "dd@account.com",
                Password = "123asdD@",
                RememberMe = true,
            };
        }

        [Theory]
        [MemberData(nameof(RequestValidatorTestData.InvalidEmailData), MemberType = typeof(RequestValidatorTestData))]
        public void Should_have_error_when_Email_is_invalid(string email)
        {
            var invalidRequest = DeepCopier.Copy(_validRequest);
            invalidRequest.Email = email;

            var result = _validator.TestValidate(invalidRequest);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [MemberData(nameof(RequestValidatorTestData.InvalidPswdData), MemberType = typeof(RequestValidatorTestData))]
        public void Should_have_error_when_Password_is_invalid(string password)
        {
            var invalidRequest = DeepCopier.Copy(_validRequest);
            invalidRequest.Password = password;

            var result = _validator.TestValidate(invalidRequest);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_not_have_errors()
        {
            var result = _validator.TestValidate(_validRequest);
            result.ShouldNotHaveAnyValidationErrors();

            _validRequest.RememberMe = false;
            result = _validator.TestValidate(_validRequest);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
