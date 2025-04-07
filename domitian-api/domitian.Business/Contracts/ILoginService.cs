using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian.Models.Results;

namespace domitian.Business.Contracts
{
    public interface ILoginService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
        Task<Result<LoginResponse>> RefreshAccessAsync(RefreshRequest refReq);
        Task<Result> RevokeAccessAsync(string? username);
    }
}
