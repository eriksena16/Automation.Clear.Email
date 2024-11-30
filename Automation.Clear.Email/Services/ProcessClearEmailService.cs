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

        public void DeleteSpamEmails()
        {
            using (var client = new ImapClient())
            {
                _logger.LogInformation("Conecta ao servidor IMAP");
                client.Connect(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.SslOnConnect);

                _logger.LogInformation("Autentica no servidor");
                client.Authenticate(_emailSettings.Email, _emailSettings.Password);

                //foreach (var folder in client.GetFolders(client.PersonalNamespaces[0]))
                //{
                //    Console.WriteLine(folder.FullName);
                //}

                _logger.LogInformation("Abrir pasta Spam");

                var spamFolder = client.GetFolder("[Gmail]/Spam");
                spamFolder.Open(FolderAccess.ReadWrite);

                _logger.LogInformation("Defina o critério de busca para os e-mails que deseja excluir");
                var searchQuery = SearchQuery.DeliveredBefore(DateTime.Now.AddDays(-7));

                _logger.LogInformation("Buscar mensagens na pasta de spam que correspondem ao critério de busca");
                var uids = spamFolder.Search(searchQuery);

                _logger.LogInformation("Apagar as mensagens encontradas");

                foreach (var uid in uids)
                {
                    spamFolder.AddFlags(uid, MessageFlags.Deleted, true);
                }

                _logger.LogInformation("Remover permanentemente");

                spamFolder.Expunge();

                _logger.LogInformation("Desconectar do servidor");
                
                client.Disconnect(true);
            }
        }
    }
}
