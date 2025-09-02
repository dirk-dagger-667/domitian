using domitian.Infrastructure.Censure;

namespace Domitian.BusinessModels.Register
{
  public class LoginDto
  {
    [Censured]
    public string Email { get; set; }

    [Censured]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
  }
}
