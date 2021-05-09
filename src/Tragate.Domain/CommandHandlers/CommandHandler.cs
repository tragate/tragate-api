using System.Threading.Tasks;
using MediatR;
using Serilog;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.CommandHandlers
{
    public class CommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications){
            _uow = uow;
            _notifications = (DomainNotificationHandler) notifications;
            _bus = bus;
        }

        private void NotifyValidationErrors(Command message){
            foreach (var error in message.ValidationResult.Errors){
                RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        protected bool Commit(){
            if (_notifications.HasNotifications()) return false;
            var commandResponse = _uow.Commit();
            if (commandResponse.Success) return true;

            RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }

        protected void Validate(Command message){
            if (!message.IsValid()){
                NotifyValidationErrors(message);
            }
        }

        protected void RaiseError(Command message, string errorMessage){
            _bus.RaiseEvent(new DomainNotification(message.MessageType, errorMessage));
            Log.Error(errorMessage);
        }

        protected void RaiseEvent(Event message){
            _bus.RaiseEvent(message);
        }

        protected void SendCommand(Command message){
            _bus.SendCommand(message);
        }
    }
}