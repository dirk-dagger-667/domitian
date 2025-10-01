namespace domitian.Models.Requests.Users
{
  public record CreateUserRequest(
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber
    );
}
