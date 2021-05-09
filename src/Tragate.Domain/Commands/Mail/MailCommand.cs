namespace Tragate.Domain.Commands
{
    public abstract class MailCommand : Command
    {
        public string MailName { get; set; }
        public string MailTitle { get; set; }
        public string[] To { get; set; }
        public string[] Bcc { get; set; }
    }
}