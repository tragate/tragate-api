using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using Tragate.Common.Library;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.EventHandlers.Base;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class CompanyEventHandler : BaseCompanyEventHandler,
        INotificationHandler<CompanyRegisteredEvent>,
        INotificationHandler<CompanyUpdatedEvent>,
         INotificationHandler<CompanyFastAddedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ConfigSettings _settings;

        public CompanyEventHandler(ElasticClient elasticClient,
            ICompanyRepository companyRepository, IEmailService emailService, IOptions<ConfigSettings> settings)
            : base(elasticClient, companyRepository)
        {
            _emailService = emailService;
            _settings = settings.Value;
        }

        /// <summary>
        /// parent company
        /// </summary>
        /// <param name="message"></param>
        public void Handle(CompanyRegisteredEvent message)
        {
            base.IndexDocument(message.Id);
        }

        /// <summary>
        /// parent company
        /// </summary>
        /// <param name="message"></param>
        public void Handle(CompanyUpdatedEvent message)
        {
            base.IndexDocument(message.Id);
        }

        public void Handle(CompanyFastAddedEvent message)
        {
            var body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "company-register.html");

            body = body.Replace("@username", message.Fullname)
                       .Replace("@email", message.Email);

            _emailService.SendFastCompanyRegisterEmail(new List<string> { message.Email },
                 "Türkiye'nin Ýhracat Kapýsý TraGate'e Hoþgeldiniz", body);
        }
    }
}