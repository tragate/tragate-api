using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class MailController : ApiController
    {
        private readonly IMailService _mailService;

        public MailController(INotificationHandler<DomainNotification> notifications,
            IMailService mailService) : base(notifications){
            _mailService = mailService;
        }

        [HttpPost]
        [Route("emails")]
        public IActionResult Login([FromBody] MailViewModel model){
            _mailService.SendEmail(model);
            return Response("Mail has been sent");
        }
    }
}