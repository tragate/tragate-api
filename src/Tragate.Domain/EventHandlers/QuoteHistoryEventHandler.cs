using MediatR;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class QuoteHistoryEventHandler : INotificationHandler<QuoteHistoryCreatedEvent>
    {
        private readonly ConfigSettings _settings;
        private readonly IEmailService _emailService;
        private readonly ICompanyAdminRepository _companyAdminRepository;


        public QuoteHistoryEventHandler(
            IEmailService emailService,
            IOptions<ConfigSettings> settings,
            ICompanyAdminRepository companyAdminRepository){
            _settings = settings.Value;
            _emailService = emailService;
            _companyAdminRepository = companyAdminRepository;
        }

        /// <summary>
        /// send email to  all company admin emails and receiver user email
        /// </summary>
        /// <param name="message"></param>
        public void Handle(QuoteHistoryCreatedEvent message){
            var emails = _companyAdminRepository.GetCompanyAdminEmailsByCompanyId(message.CompanyId, StatusType.Active);
            emails.Add(message.ReceiverUser.Email);

            string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "quotation-message-update.html");
            body = body.Replace("@ReceiverUserName", message.ReceiverUser.FullName)
                .Replace("@SenderUserName", message.SenderUser.FullName)
                .Replace("@QuoteTitle", message.Title)
                .Replace("@CallbackUrl", $"{_settings.WebSite}/user")
                .Replace("@WebSite", _settings.WebSite)
                .Replace("@email", message.ReceiverUser.Email);
            _emailService.SendQuoteMessageUpdateEmail(emails, $"New Message : {message.Title}", body);
        }
    }
}