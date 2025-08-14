using domitian.Infrastructure.Censure;
using domitian.Models.Responses.Login;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace domitian.Models.Requests.Login
{
  public class LoginRequest
  {
    [XmlElement(ElementName = "email")]
    [Censured]
    public string Email { get; set; }

    [XmlElement(ElementName = "password")]
    [Censured]
    public string Password { get; set; }

    [XmlElement(ElementName = "rememberMe")]
    public bool RememberMe { get; set; }

    public LoginResponse Response { get; set; } = new LoginResponse()
    {
      UserId = "asd",
      BearerToken = "asd",
      RefreshToken = "asd"
    };
  }
}
