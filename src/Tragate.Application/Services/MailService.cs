using Tragate.Application.ViewModels;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;

namespace Tragate.Application
{
    public class MailService : IMailService
    {
        private readonly IMediatorHandler _bus;

        public MailService(IMediatorHandler bus){
            _bus = bus;
        }

        public void SendEmail(MailViewModel model){
            _bus.SendCommand(new SendNewMailCommand()
            {
                MailName = model.MailName,
                MailTitle = model.MailTitle,
                To = model.To
            });
        }
    }
}