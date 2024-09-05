using dominitian_ui.Models.Requests.Login;
using dominitian_ui.Models.Responses.Login;
using dominitian_ui.Models.Results;

namespace dominitian.Business.Contracts
{
    public interface ILoginService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
        Task<Result<LoginResponse>> RefreshAccessAsync(RefreshRequest refReq);
        Task<Result> RevokeAccessAsync(string? username);
    }
}
