using System.Collections.Generic;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Infrastructure;

namespace Tragate.Domain.EventHandlers
{
    public class GoldMemberHandler : IEMailHandler
    {
        private readonly IEmailService _emailService;
        private readonly ConfigSettings _settings;

        public GoldMemberHandler(IEmailService emailService, ConfigSettings settings){
            _emailService = emailService;
            _settings = settings;
        }

        public void Execute(object data){
            var model = (CompanyMembershipDetailDto) data;
            var callback = $"{_settings.WebSite}/{model.Slug}/home";
            string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "gold-member.html");
            body = body.Replace("@username", model.UserName)
                .Replace("@companyname", model.CompanyName)
                .Replace("@email", model.UserEmail)
                .Replace("@membershipType", model.MembershipPackage)
                .Replace("@startDate", model.StartDate.ToString("dd/MM/yyyy HH:mm:ss"))
                .Replace("@endDate", model.EndDate.ToString("dd/MM/yyyy HH:mm:ss"))
                .Replace("@callback", callback)
                .Replace("@website", _settings.WebSite);
            _emailService.SendActivationEmail(new List<string> {model.UserEmail},
                "Welcome to TraGate Gold Company Programme", body);
        }
    }
}