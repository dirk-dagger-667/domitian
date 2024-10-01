using DeepCopy;
using domitian.Infrastructure.Validators;
using domitian.Models.Requests.Registration;
using FluentValidation.TestHelper;

namespace domitian.Infrastructure.Tests.Validators
{
    public class ConfirmEmailRequestValidatorTester
    {
        private readonly ConfirmEmailRequestValidator _validator;
        private readonly ConfirmEmailRequest _validRequest;

        public ConfirmEmailRequestValidatorTester()
        {
            _validator = new ConfirmEmailRequestValidator();
            _validRequest = new ConfirmEmailRequest()
            {
                UserId = "123123asd3rt53H*D#&&!HDDH82hujhskr289`82h",
                Code = "198723uy1u2h4172ihI&H@&!2uh12i74i7h12p19dp)(_(@UJ!~PWID8oh3OF*@HO~!Hdo*@#O@*do*@dy",
            };
        }

        [Fact]
        public void Should_not_have_any_errors()
        {
            var result = _validator.TestValidate(_validRequest);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_be_errors_if_UserId_is_invalid(string userId)
        {
            var invalidRequest = DeepCopier.Copy(_validRequest);
            invalidRequest.UserId = userId;

            var result = _validator.TestValidate(invalidRequest);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_be_errors_if_Code_is_invalid(string code)
        {
            var invalidRequest = DeepCopier.Copy(_validRequest);
            invalidRequest.Code = code;

            var result = _validator.TestValidate(invalidRequest);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }
    }
}
