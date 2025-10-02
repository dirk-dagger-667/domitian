using domitian.Models.Dtos.UserAdmin;
using domitian.Models.Requests.Users;
using domitian.Models.Responses.UserAdmin;
using domitian.Models.Results;

namespace domitian.Business.Contracts
{
  public interface IUsersService
  {
    Task<Result<UserDto>> GetAsync(string id);
    Task<Result<UsersResponse>> GetPageAsync(PaganationParameters pageParams);
    Task<Result> UpdateBulkAsync(IEnumerable<UpdateUserRequest> updateUserRequests);
    Task<Result> UpdateAsync(string id, UpdateUserRequest updateUserRequest);
    Task<Result> DeleteBulkAsync(IEnumerable<string> ids);
    Task<Result> DeleteAsync(string id);
  }
}
