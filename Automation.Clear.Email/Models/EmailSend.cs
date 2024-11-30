namespace Automation.Clear.Email.Models
{
    public class EmailSend
    {
        public EmailSend()
        {
            Destinatarios = [];
        }

        public List<string> Destinatarios { get; set; }
        public string Assunto { get; set; }
        public string Menssagem { get; set; }
        public string HtmlString { get; set; }
    }
}
