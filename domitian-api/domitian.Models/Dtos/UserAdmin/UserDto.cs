namespace domitian.Models.Dtos.UserAdmin
{
  public record UserDto(
    string Id,
    string Email,
    bool Confirmed,
    string FullName,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastModified,
    string? PhoneNumber,
    DateTimeOffset? DeletedAt
    );
}
