using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers {
    public class ContentController : ApiController {
        private readonly IContentService _contentService;
        public ContentController (IContentService contentService,
            INotificationHandler<DomainNotification> notifications) : base (notifications) {
            _contentService = contentService;
        }

        [HttpGet]
        [Route ("contents")]
        public IActionResult GetContents () {
            return Response (_contentService.GetAll ());
        }

        [HttpGet]
        [Route ("contents/{id}")]
        public IActionResult GetContentById (int id) {
            return Response (_contentService.GetById (id));
        }

        [HttpGet]
        [Route ("contents/slug={slug}/statusId={statusId:int:min(0)}")]
        public IActionResult GetContentBySlug (string slug, int statusId) {
            return Response (_contentService.GetBySlug (slug, statusId));
        }

        [HttpPost]
        [Route ("contents")]
        public IActionResult AddContent ([FromBody] ContentViewModel model) {
            _contentService.AddContent (model);
            return Response (null, "Content has been created");
        }

        [HttpPut]
        [Route ("contents")]
        public IActionResult UpdateContent ([FromBody] ContentViewModel model) {
            _contentService.UpdateContent (model);
            return Response (null, "Content has been updated");
        }
    }
}