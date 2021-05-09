using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Tragate.Application;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers {
    public class ParamaterController : ApiController {
        private readonly IParameterService _parameterService;
        public ParamaterController (INotificationHandler<DomainNotification> notifications,
            IParameterService parameterService) : base (notifications) {
            _parameterService = parameterService;
        }

        [HttpGet]
        [Route ("parameters/{type}")]
        public IActionResult GetParameterByType (string type, int statusId) {
            var result = _parameterService.GetParameterByType (type, statusId);
            return Response (result);
        }
    }
}