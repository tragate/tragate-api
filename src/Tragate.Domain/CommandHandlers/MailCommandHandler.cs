using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.CommandHandlers
{
    public class MailCommandHandler : CommandHandler,
        INotificationHandler<SendNewMailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly ConfigSettings _settings;

        public MailCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            IOptions<ConfigSettings> settings,
            INotificationHandler<DomainNotification> notifications,
            IEmailService emailService) : base(uow, bus, notifications){
            _emailService = emailService;
            _settings = settings.Value;
        }

        public async void Handle(SendNewMailCommand message){
            base.Validate(message);
            if (message.IsValid()){
                string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, message.MailName);
                foreach (var to in message.To.ToList()){
                    await _emailService.GeneralSendEmail(new List<string>() {to}, message.MailTitle, body);
                }
            }
        }
    }
}