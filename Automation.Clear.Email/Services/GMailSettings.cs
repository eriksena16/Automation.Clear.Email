namespace Automation.Clear.Email.Services
{
    public class GMailSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}