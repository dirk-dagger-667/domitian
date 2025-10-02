using FluentValidation;

namespace domitian_api.Validators
{
  public class StringCollectionValidator : AbstractValidator<IEnumerable<string>>
  {
    public StringCollectionValidator()
    {
      RuleFor(x => x).NotNull();
    }
  }
}
