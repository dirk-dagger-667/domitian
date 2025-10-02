using domitian.Business.Contracts;
using domitian.Business.Mappers;
using domitian.Data.Identity;
using domitian.Models.Dtos.UserAdmin;
using domitian.Models.Requests.Users;
using domitian.Models.Responses.UserAdmin;
using domitian.Models.Results;
using domitian.Models.Results.Errors;
using Microsoft.EntityFrameworkCore;

namespace domitian.Business.Services.UsersService
{
  public class UsersService(DomitianIDDbContext context) : IUsersService
  {
    public async Task<Result> DeleteAsync(string id)
    {
      var result = await context.Users
        .Where(user => user.Id == id)
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(u => u.DeletedAt, TimeProvider.System.GetLocalNow())
        .SetProperty(u => u.LastModifiedAt, TimeProvider.System.GetLocalNow()));

      if (result < 1)
        return Result.Failure(DevOperationErrorMessages.OperationFailed, ResultType.NotFound, UsersErrors.UsersNotFound);

      return Result.NoContent();
    }

    public async Task<Result> DeleteBulkAsync(IEnumerable<string> ids)
    {
      var result = await context.Users
        .Where(user => ids.Contains(user.Id))
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(u => u.DeletedAt, TimeProvider.System.GetLocalNow())
        .SetProperty(u => u.LastModifiedAt, TimeProvider.System.GetLocalNow()));

      if (result < 1)
        return Result<int>.Failure(DevOperationErrorMessages.OperationFailed,
          ResultType.NotFound,
          UsersErrors.UsersNotFound);

      return Result.NoContent();
    }

    public async Task<Result<UserDto>> GetAsync(string id)
    {
      var user = await context.Users
        .AsNoTracking()
        .Where(user => user.Id == id)
        .Select(user => user.MapToUserDto())
        .SingleAsync();

      if (user is null)
        return Result<UserDto>.Failure(DevOperationErrorMessages.OperationFailed
          , ResultType.NotFound,
          UsersErrors.UsersNotFound);

      return Result<UserDto>.Success(user);
    }

    public async Task<Result<UsersResponse>> GetPageAsync(PaganationParameters pageParams)
    {
      var query = context.Users
          .AsNoTracking()
          .OrderBy(u => u.UserName)
          .Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
          .Take(pageParams.PageSize)
          .Select(user => user.MapToUserDto());

      var count = await query.CountAsync();
      var users = query.AsAsyncEnumerable();

      return Result<UsersResponse>.Success(new UsersResponse(users, count, pageParams.PageNumber));
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest updateUserRequest)
    {
      var result = await context.Users
        .Where(user => user.Id == id)
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(u => u.Email, u => updateUserRequest.Email ?? u.Email)
        .SetProperty(u => u.FirstName, u => updateUserRequest.FirstName ?? u.FirstName)
        .SetProperty(u => u.LastName, u => updateUserRequest.LastName ?? u.LastName)
        .SetProperty(u => u.PhoneNumber, u => updateUserRequest.PhoneNumber ?? u.PhoneNumber)
        .SetProperty(u => u.LastModifiedAt, TimeProvider.System.GetLocalNow()));

      if(result < 1)
        return Result.Failure(DevOperationErrorMessages.OperationFailed,
          ResultType.NotFound,
          UsersErrors.UsersNotFound);

      return Result.NoContent();
    }

    public async Task<Result> UpdateBulkAsync(IEnumerable<UpdateUserRequest> updateUserRequests)
    {
      var updateMap = updateUserRequests.ToDictionary(u => u.Id);

      var result = await context.Users
        .Where(user => updateMap.Keys.Contains(user.Id))
        .ExecuteUpdateAsync(setters => setters
        .SetProperty(u => u.Email, u => updateMap[u.Id].Email ?? u.Email)
        .SetProperty(u => u.FirstName, u => updateMap[u.Id].FirstName ?? u.FirstName)
        .SetProperty(u => u.LastName, u => updateMap[u.Id].LastName ?? u.LastName)
        .SetProperty(u => u.PhoneNumber, u => updateMap[u.Id].PhoneNumber ?? u.PhoneNumber)
        .SetProperty(u => u.LastModifiedAt, TimeProvider.System.GetLocalNow()));

      if (result < 1)
        return Result.Failure(DevOperationErrorMessages.OperationFailed,
          ResultType.NotFound, UsersErrors.UsersNotFound);

      return Result.NoContent();
    }
  }
}
