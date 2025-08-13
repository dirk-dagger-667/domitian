using domitian.Infrastructure.Censure;

namespace domitian.Models.Requests.Login
{
  public record RefreshRequest
  {
    [Censured]
    public required string AccessToken { get; init; }

    [Censured]
    public required string RefreshToken { get; init; }
  }
}
