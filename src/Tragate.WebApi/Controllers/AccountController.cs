using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tragate.Application;
using Tragate.Application.ViewModels;
using Tragate.CrossCutting.Filters;
using Tragate.Domain.Core.Notifications;

namespace Tragate.WebApi.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        public AccountController(
            INotificationHandler<DomainNotification> notifications,
            IUserService userService) : base(notifications){
            _userService = userService;
        }

        [HttpPost]
        [Route("account/login")]
        [ServiceFilter(typeof(AnonymUserValidationFilterAttribute))]
        public IActionResult Login([FromBody] LoginViewModel model){
            var result = _userService.Login(model);
            return Response(result);
        }

        [HttpPost]
        [Route("account/external-login")]
        public IActionResult ExternalLogin([FromBody] ExternalLoginViewModel model){
            return Response(_userService.ExternalLogin(model));
        }

        [HttpPost]
        [Route("account/verify")]
        public IActionResult UserVerify([FromBody] VerifyViewModel model){
            _userService.Verify(model);
            return Response(null, "User has been verified");
        }

        [HttpGet]
        [Route("account/{token}")]
        public IActionResult GetUserByToken(string token){
            var result = _userService.GetUserByVerifiedToken(token);
            return Response(result);
        }

        [HttpPost]
        [Route("account/signup")]
        [ServiceFilter(typeof(AnonymUserValidationFilterAttribute))]
        public IActionResult UserRegister([FromBody] PersonViewModel model){
            _userService.PersonRegister(model);
            return Response(null, "Person has been created");
        }

        [HttpPost]
        [Route("account/complete-signup")]
        public IActionResult CompleteSignUp([FromBody] CompleteSignUpViewModel model){
            _userService.CompleteSignUp(model);
            return Response(null, "Your account has been completed");
        }

        [HttpPost]
        [Route("account/external-signup")]
        [ServiceFilter(typeof(ExternalUserValidationFilterAttribute))]
        public IActionResult ExternalSignUp([FromBody] ExternalSignUpViewModel model){
            _userService.ExternalSignUp(model);
            return Response(null, "External user has been created");
        }

        [HttpPost]
        [Route("account/forgot-password")]
        [ServiceFilter(typeof(AnonymUserValidationFilterAttribute))]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordViewModel model){
            _userService.ForgotPassword(model);
            return Response(null, "Email has been sent");
        }

        [HttpPost]
        [Route("account/reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordViewModel model){
            _userService.ResetPassword(model);
            return Response(null, "Your password has been resetted");
        }
    }
}