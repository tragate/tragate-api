using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Result;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.CrossCutting.Filters
{
    public class AnonymUserValidationFilterAttribute : IActionFilter
    {
        private readonly IUserRepository _userRepository;
        private readonly ConfigSettings _settings;
        private readonly IMediatorHandler _bus;

        public AnonymUserValidationFilterAttribute(IUserRepository userRepository, IOptions<ConfigSettings> settings,
            IMediatorHandler bus){
            _userRepository = userRepository;
            _bus = bus;
            _settings = settings.Value;
        }

        public void OnActionExecuting(ActionExecutingContext context){
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is AnonymUserViewModel);
            if (param.Value != null){
                var model = param.Value as AnonymUserViewModel;
                var user = _userRepository.GetByEmail(model.Email);
                if (!string.IsNullOrEmpty(model.Email) && user != null){
                    switch (user.RegisterType){
                        case RegisterType.Anonymous:
                            AnonymUserValidate(context, user);
                            break;
                        case RegisterType.Facebook:
                        case RegisterType.Google:
                        case RegisterType.Linkedin:
                            ExternalUserValidate(context, user);
                            break;
                    }
                }
            }
        }

        private void ExternalUserValidate(ActionExecutingContext context, UserDto user){
            context.Result = new BadRequestObjectResult(new BadResult
            {
                Success = false,
                Errors = new List<string>()
                {
                    $"you have been signed in with {user.RegisterTypeName}"
                }
            });
        }

        private void AnonymUserValidate(ActionExecutingContext context, UserDto user){
            context.Result = new BadRequestObjectResult(new BadResult
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Email doesn't exists"
                },
                Links = new List<Link>()
                {
                    new Link()
                    {
                        Href = $"{_settings.WebSite}/account/complete-signup-info",
                        Rel = "account/complete-signup"
                    }
                }
            });

            _bus.RaiseEvent(new AnonymUserCreatedEvent() {UserId = user.Id});
        }

        public void OnActionExecuted(ActionExecutedContext context){
        }
    }
}