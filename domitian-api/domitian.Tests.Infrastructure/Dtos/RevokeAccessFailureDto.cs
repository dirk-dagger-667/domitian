using domitian.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace domitian.Tests.Infrastructure.Dtos
{
  public record RevokeAccessUnauthorizedDto
  {
    public string? UserName { get; init; }
    public DomitianIDUser? User { get; init; }
    public IdentityResult? UpdRes { get; init; }
  }
}
