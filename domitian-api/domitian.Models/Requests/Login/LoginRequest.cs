using domitian.Infrastructure.Censure;
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
  }
}
