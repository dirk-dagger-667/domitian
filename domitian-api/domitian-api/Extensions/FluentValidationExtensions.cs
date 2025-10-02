using FluentValidation.Results;

namespace domitian_api.Extensions
{
  public static class FluentValidationExtensions
  {
    public static string ConcatErrors(this IEnumerable<ValidationFailure> valFails)
        => string.Join(Environment.NewLine, valFails.Select(failure => failure.ErrorMessage));

    public static IDictionary<string, string[]> ToDictionary(this ValidationResult valResult)
    => valResult.Errors.GroupBy(e => e.PropertyName)
        .ToDictionary(g => g.Key,
        g => g.Select(x => x.ErrorMessage).ToArray());
  }
}
