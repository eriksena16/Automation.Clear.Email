using Automation.Clear.Email.Models;

namespace Automation.Clear.Email.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSend email);
    }
}
