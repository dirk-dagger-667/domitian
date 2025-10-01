using Microsoft.AspNetCore.Identity;

namespace domitian.Data.Identity
{
  public class DomitianIDUser : IdentityUser
  {
    //[Censured]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }

    //[Censured]
    public string? FirstName { get; set; }

    //[Censured]
    public string? LastName { get; set; }

    public DateTimeOffset LastModifiedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
  }
}
