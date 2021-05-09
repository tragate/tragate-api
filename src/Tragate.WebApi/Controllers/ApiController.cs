using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tragate.Common.Result;
using Tragate.Domain.Core.Notifications;
using OkResult = Tragate.Common.Result.OkResult;


namespace Tragate.WebApi.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    public abstract class ApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;

        protected ApiController(INotificationHandler<DomainNotification> notifications){
            _notifications = (DomainNotificationHandler) notifications;
        }

        private bool IsValidOperation(){
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null, string message = null, List<Link> link = null){
            if (IsValidOperation()){
                return Ok(new OkResult
                {
                    Success = true,
                    Data = result,
                    Message = message,
                    Links = link
                });
            }

            return BadRequest(new BadResult
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyModelStateErrors(){
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros){
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        private void NotifyError(string code, string message){
            _notifications.Handle(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result){
            foreach (var error in result.Errors){
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}