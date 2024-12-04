using Automation.Clear.Email.Services.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Automation.Clear.Email.Services
{
    public class ProcessClearEmailService : IProcessClearEmailService
    {
        private readonly GMailSettings _emailSettings;
        private readonly ILogger<ProcessClearEmailService> _logger;

        public ProcessClearEmailService(IOptions<GMailSettings> emailSettings, ILogger<ProcessClearEmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public void ProcessEmails()
        {
            using (var client = new ImapClient())
            {
                _logger.LogInformation("Conecta ao servidor IMAP");
                client.Connect(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.SslOnConnect);

                _logger.LogInformation("Autentica no servidor");
                client.Authenticate(_emailSettings.Email, _emailSettings.Password);

                foreach (var folder in client.GetFolders(client.PersonalNamespaces[0]))
                {
                    Console.WriteLine(folder.FullName);
                }

                DeleteAllEmails(client);
                DeleteEmailsNaoAbertos(client);

               

                _logger.LogInformation("Desconectar do servidor");

                client.Disconnect(true);
            }
        }

        private void DeleteAllEmails(ImapClient client)
        {

            var emailFolders = new List<IMailFolder>() { client.GetFolder("WTF"), client.GetFolder("LuxOne"), client.GetFolder("[Gmail]/Spam"), client.GetFolder("Nubank"), client.GetFolder("Estácio") };

            foreach (var emailFolder in emailFolders)
            {
                _logger.LogInformation($"Abrir pasta {emailFolder.FullName}");

                emailFolder.Open(FolderAccess.ReadWrite);

                _logger.LogInformation("Defina o critério de busca para os e-mails que deseja excluir");
                var searchQuery = SearchQuery.All;

                _logger.LogInformation("Buscar mensagens na pasta de spam que correspondem ao critério de busca");
                var uids = emailFolder.Search(searchQuery);

                _logger.LogInformation("Apagar as mensagens encontradas");

                foreach (var uid in uids)
                {
                    var message = emailFolder.GetMessage(uid);
                    _logger.LogInformation($"Excluindo o e-mail com assunto: {message.Subject}, De: {message.From}");

                    emailFolder.AddFlags(uid, MessageFlags.Deleted, true);
                }

                _logger.LogInformation($"Emails removidos da pasta: {emailFolder.FullName} - {uids.Count}");

                emailFolder.Expunge();
            }

        }
        private void DeleteEmailsNaoAbertos(ImapClient client)
        {
            var emailFolder = client.GetFolder("INBOX");

            _logger.LogInformation($"Abrir pasta {emailFolder.FullName}");

            emailFolder.Open(FolderAccess.ReadWrite);

            _logger.LogInformation("Defina o critério de busca para os e-mails que deseja excluir");

            var searchQuery = SearchQuery.NotSeen.And(SearchQuery.DeliveredBefore(DateTime.Now.AddMonths(-1)));

            _logger.LogInformation("Buscar mensagens na pasta de spam que correspondem ao critério de busca");
            var uids = emailFolder.Search(searchQuery);

            _logger.LogInformation("Apagar as mensagens encontradas");

            foreach (var uid in uids)
            {
                var message = emailFolder.GetMessage(uid);
                _logger.LogInformation($"Excluindo o e-mail com assunto: {message.Subject}, De: {message.From}");

                emailFolder.AddFlags(uid, MessageFlags.Deleted, true);
            }

            _logger.LogInformation($"Emails removidos da pasta: {emailFolder.FullName} - {uids.Count}");

            emailFolder.Expunge();


        }
    }
}
