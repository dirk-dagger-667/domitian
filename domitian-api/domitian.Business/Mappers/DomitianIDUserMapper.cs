using domitian.Data.Identity;
using domitian.Models.Dtos.UserAdmin;

namespace domitian.Business.Mappers
{
  public static class DomitianIDUserMapper
  {
    public static UserDto MapToUserDto(this DomitianIDUser domitianUser)
    => new UserDto(
        Id: domitianUser.Id,
        Email: domitianUser.Email,
        Confirmed: domitianUser.EmailConfirmed,
        FullName: $"{domitianUser.FirstName} {domitianUser.LastName}",
        CreatedAt: domitianUser.CreatedAt,
        LastModified: domitianUser.LastModifiedAt,
        PhoneNumber: domitianUser.PhoneNumber,
        DeletedAt: domitianUser.DeletedAt);
  }
}
