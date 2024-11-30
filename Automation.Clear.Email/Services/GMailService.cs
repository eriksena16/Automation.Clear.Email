//using Automation.Clear.Email.Models;
//using MailKit.Net.Smtp;
//using Microsoft.Extensions.Options;
//using MimeKit;

//namespace Automation.Clear.Email.Services
//{
//    public class GMailService : IEmailService
//    {
//        private readonly GMailSettings _emailSettings;

//        public GMailService(IOptions<GMailSettings> emailSettings)
//        {
//            _emailSettings = emailSettings.Value;
//        }

//        public async Task SendEmailAsync(EmailSend email)
//        {
//            var mensagem = new MimeMessage();
//            mensagem.From.Add(new MailboxAddress(_emailSettings.NomeRemetente, _emailSettings.Email));
//            mensagem.To.AddRange(email.Destinatarios.Select( MailboxAddress.Parse));
//            mensagem.Subject = email.Assunto;
//            var builder = new BodyBuilder { TextBody = email.Menssagem, HtmlBody = email.HtmlString };
//            mensagem.Body = builder.ToMessageBody();
//            try
//            {
//                var smtpClient = new SmtpClient
//                {
//                    ServerCertificateValidationCallback = (s, c, h, e) => true
//                };
//                await smtpClient.ConnectAsync(_emailSettings.Server, _emailSettings.Port).ConfigureAwait(false);
//                await smtpClient.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password).ConfigureAwait(false);
//                await smtpClient.SendAsync(mensagem).ConfigureAwait(false);
//                await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException(ex.Message);
//            }
//        }
//    }
//}