using System.Xml.Serialization;

namespace domitian.Models.Requests.Login
{
  public class LoginRequest
  {
    [XmlElement(ElementName = "email")]
    public string Email { get; set; }

    [XmlElement(ElementName = "password")]
    public string Password { get; set; }

    [XmlElement(ElementName = "rememberMe")]
    public bool RememberMe { get; set; }
  }
}
