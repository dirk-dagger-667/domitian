using domitian.Models.Dtos.UserAdmin;

namespace domitian.Models.Responses.UserAdmin
{
  public record UsersResponse(
    IAsyncEnumerable<UserDto> Users,
    int Count,
    int PageNumber);
}
