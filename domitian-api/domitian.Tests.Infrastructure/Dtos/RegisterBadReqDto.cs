using domitian.Data.Identity;
using domitian.Models.Results.Errors;
using Microsoft.AspNetCore.Identity;

namespace domitian.Tests.Infrastructure.Dtos
{
  public record RegisterBadReqDto
  {
    public DomitianIDUser? User { get; init; }
    public IdentityResult? IdRes { get; init; }
    public bool SupportsEmail { get; init; }
    public IdentityResult? AddToRoleRes { get; init; }
    public required Error Error { get; init; }
  }
}
