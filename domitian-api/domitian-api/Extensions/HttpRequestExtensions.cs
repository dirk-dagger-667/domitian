namespace domitian_api.Extensions
{
  public static class HttpRequestExtensions
  {
    public static string ToUriString(this HttpRequest httpRequest)
      => $"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.Path}{httpRequest?.QueryString}";
  }
}
