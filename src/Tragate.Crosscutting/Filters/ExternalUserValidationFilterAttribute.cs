using System.Linq;
using Tragate.Common.Library;
using Tragate.Common.Result;
using Tragate.Domain.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Tragate.Application.ViewModels;

namespace Tragate.CrossCutting.Filters
{
    public class ExternalUserValidationFilterAttribute : IActionFilter
    {
        private readonly IUserRepository _userRepository;
        private readonly ConfigSettings _settings;

        public ExternalUserValidationFilterAttribute(IUserRepository userRepository, IOptions<ConfigSettings> settings){
            _userRepository = userRepository;
            _settings = settings.Value;
        }

        public void OnActionExecuting(ActionExecutingContext context){
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is ExternalSignUpViewModel);
            if (param.Value != null){
                var model = param.Value as ExternalSignUpViewModel;
                var user = _userRepository.GetUserByExternalUserId(model.ExternalUserId, model.Email);
                if (user != null){
                    context.Result = new BadRequestObjectResult(new BadResult
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "User already exist"
                        },
                        Links = new List<Link>()
                        {
                            new Link()
                            {
                                Href = $"{_settings.WebSite}/account/external-login",
                                Rel = "account/external-login"
                            }
                        }
                    });
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){
        }
    }
}