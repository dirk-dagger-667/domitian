using domitian.Business.Contracts;
using domitian.Business.Extensions;
using domitian.Models.Dtos.UserAdmin;
using domitian.Models.Requests.Users;
using domitian.Models.Responses.UserAdmin;
using domitian.Models.Results;
using domitian_api.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Services.UsersService
{
  public class UsersServiceCC(
    [FromKeyedServices(AppConstants.InnerKey)] IUsersService inner,
    ILogger<UsersService> logger) : IUsersService
  {
    public async Task<Result> DeleteAsync(string id)
    {
      var result = await inner.DeleteAsync(id);
      logger.LogResult(result, nameof(DeleteAsync), nameof(IUsersService), id);

      return result;
    }

    public async Task<Result> DeleteBulkAsync(IEnumerable<string> ids)
    {
      var result = await inner.DeleteBulkAsync(ids);
      logger.LogResult(result, nameof(DeleteBulkAsync), nameof(IUsersService), ids);

      return result;
    }

    public async Task<Result<UserDto>> GetAsync(string id)
    {
      var result = await inner.GetAsync(id);
      logger.LogResult(result, nameof(GetAsync), nameof(IUsersService), id);

      return result;
    }

    public async Task<Result<UsersResponse>> GetPageAsync(PaganationParameters pageParams)
    {
      var result = await inner.GetPageAsync(pageParams);
      logger.LogResult(result, nameof(GetPageAsync), nameof(IUsersService), pageParams);

      return result;
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest updateUserRequest)
    {
      var result = await inner.UpdateAsync(id, updateUserRequest);
      logger.LogResult(result, nameof(UpdateAsync), nameof(IUsersService), new { id, updateUserRequest } );

      return result;
    }

    public async Task<Result> UpdateBulkAsync(IEnumerable<UpdateUserRequest> updateUserRequests)
    {
      var result = await inner.UpdateBulkAsync(updateUserRequests);
      logger.LogResult(result, nameof(UpdateBulkAsync), nameof(IUsersService), updateUserRequests);

      return result;
    }
  }
}
