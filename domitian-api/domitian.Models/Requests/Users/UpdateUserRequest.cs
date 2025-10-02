namespace domitian.Models.Requests.Users
{
  public record UpdateUserRequest(
    string Id,
    string? Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber
    );
}
