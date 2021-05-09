using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers {
    public class LocationController : ApiController {
        private readonly ILocationService _locationService;
        public LocationController (INotificationHandler<DomainNotification> notifications,
            ILocationService locationService) : base (notifications) {
            _locationService = locationService;
        }

        [HttpGet]
        [Route ("locations")]
        public IActionResult GetLocationByParentId (int? parentId, int statusId) {
            var result = _locationService.GetLocationByParentId (parentId, statusId);
            return Response (result);
        }
    }
}