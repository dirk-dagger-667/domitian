using domitian.Models.Requests.Registration;
using domitian.Models.Results;

namespace domitian.Business.Contracts
{
    public interface IRegisterService
    {
        Task<Result<string>> RegisterAsync(RegisterRequest request);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result<string>> ConfirmRegistrationAsync(string email);
    }
}
