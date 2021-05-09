using Tragate.Application.ViewModels;

namespace Tragate.Application
{
    public interface IMailService
    {
        void SendEmail(MailViewModel model);
    }
}