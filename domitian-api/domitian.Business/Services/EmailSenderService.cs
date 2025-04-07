using Microsoft.AspNetCore.Identity.UI.Services;

namespace domitian.Business.Services
{
    public class EmailSenderService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
