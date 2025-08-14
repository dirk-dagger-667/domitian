using domitian.Infrastructure.Censure;

namespace domitian.Models.Responses.Login
{
  public record LoginResponse
  {
    public required string? UserId { get; init; }

    [Censured]
    public required string? BearerToken { get; init; }

    [Censured]
    public required string? RefreshToken { get; init; }
  }
}
