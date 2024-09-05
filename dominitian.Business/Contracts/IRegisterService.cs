using dominitian_ui.Models.Requests.Registration;
using dominitian_ui.Models.Results;

namespace dominitian.Business.Contracts
{
    public interface IRegisterService
    {
        Task<Result<string>> RegisterAsync(RegisterRequest request);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result<string>> ConfirmRegistrationAsync(string email);
    }
}
