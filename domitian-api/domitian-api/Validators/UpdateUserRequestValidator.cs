using domitian.Models.Requests.Users;
using FluentValidation;

namespace domitian_api.Validators
{
  public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
  {
    public const string PhoneNumberRegexExpression = "^(\\+?\\d{1,3})?[-.\\s]?(\\d{9,10})$";

    public UpdateUserRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

      RuleFor(x => x.Id).NotEmpty();

      RuleFor(x => x.Email)
        .NotEmpty()
        .Matches(RegisterRequestValidator.EmailRegexExpression)
        .When(x => x.Email is not null);

      RuleFor(x => x.FirstName)
        .NotEmpty()
        .When(x => x.FirstName is not null);

      RuleFor(x => x.LastName)
        .NotEmpty()
        .When(x => x.LastName is not null);

      RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .Matches(PhoneNumberRegexExpression)
        .When(x => x.PhoneNumber is not null);
    }
  }
}
