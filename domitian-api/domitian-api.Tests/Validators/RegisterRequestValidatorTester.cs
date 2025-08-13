using DeepCopy;
using domitian.Infrastructure.Tests.DataSources;
using domitian.Models.Requests.Registration;
using domitian_api.Validators;
using FluentValidation.TestHelper;

namespace domitian_api.Tests.Unit.Validators
{
    public class RegisterRequestValidatorTester
    {
        private readonly RegisterRequestValidator _validator;
        private readonly RegisterRequest _validRequest;

        public RegisterRequestValidatorTester()
        {
            _validator = new RegisterRequestValidator();
            _validRequest = new RegisterRequest()
            {
                Email = "dd@account.com",
                Password = "123asdD@",
                ConfirmPassword = "123asdD@"
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

        [Theory]
        [MemberData(nameof(RequestValidatorTestData.InvalidCnfrmPswd), MemberType = typeof(RequestValidatorTestData))]
        public void Should_have_error_when_ConfirmPassword_is_ivalid_and_not_the_same_as_Password(string confirmPassword)
        {
            var invalidRequest = DeepCopier.Copy(_validRequest);
            invalidRequest.ConfirmPassword = confirmPassword;

            var result = _validator.TestValidate(invalidRequest);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [Fact]
        public void Should_not_have_errors()
        {
            var result = _validator.TestValidate(_validRequest);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
