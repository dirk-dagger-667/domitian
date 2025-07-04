using domitian.Models.Requests.Registration;
using FluentValidation;

namespace domitian.Infrastructure.Validators
{
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {
    public const string PasswordRegextExpression = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$";
    public const string EmailRegexExpression = "(?:[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";

    public RegisterRequestValidator()
    {
      RuleLevelCascadeMode = CascadeMode.Stop;

      RuleFor(x => x.Email).NotEmpty().Matches(EmailRegexExpression);
      RuleFor(x => x.Password).NotEmpty().Length(8, 24).Matches(PasswordRegextExpression);
      RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password);
    }
  }
}
