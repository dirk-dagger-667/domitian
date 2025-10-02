using domitian.Models.Requests.Users;
using FluentValidation;

namespace domitian_api.Validators
{
  public class UpdateUserRequestCollectionValidator : AbstractValidator<IEnumerable<UpdateUserRequest>>
  {
    public UpdateUserRequestCollectionValidator(IValidator<UpdateUserRequest> validator)
    {
      RuleFor(x => x).NotEmpty();
      RuleForEach(x => x)
        .SetValidator(validator)
        .When(x => x is not null || x?.Count() > 0);
    }
  }
}
