using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class CompanyMembershipController : ApiController
    {
        private readonly ICompanyMembershipService _companyMembershipService;

        public CompanyMembershipController(INotificationHandler<DomainNotification> notifications,
            ICompanyMembershipService companyMembershipService) :
            base(notifications){
            _companyMembershipService = companyMembershipService;
        }

        [HttpPost]
        [Route("company-memberships")]
        public IActionResult AddCompanyMembership([FromBody] CompanyMembershipViewModel model){
            _companyMembershipService.AddCompanyMembership(model);
            return Response(null, "Firma üyeliği başarıyla eklendi");
        }
    }
}