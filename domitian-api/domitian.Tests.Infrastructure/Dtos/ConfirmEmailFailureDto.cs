using domitian.Data.Identity;
using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian.Models.Results.Errors;
using Microsoft.AspNetCore.Identity;

namespace domitian.Tests.Infrastructure.Dtos
{
  public record ConfirmEmailFailureDto
  {
    public required ConfirmEmailRequest ConfEmailReq { get; init; }
    public DomitianIDUser? User { get; init; }
    public IdentityResult? IdRes { get; init; }
    public required Error Error { get; set; }
    public required Action<Result, Error> Assertion { get; init; }
  }
}
