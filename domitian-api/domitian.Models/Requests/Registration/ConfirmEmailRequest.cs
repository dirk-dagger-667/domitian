using domitian.Infrastructure.Censure;

namespace domitian.Models.Requests.Registration
{
  public class ConfirmEmailRequest
  {
    public string UserId { get; set; }

    [Censured]
    public string Code { get; set; }
  }
}
