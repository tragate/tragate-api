using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateUserCommandValidation : UserValidation<UpdateUserCommand> {
        public UpdateUserCommandValidation () {
            ValidateId ();
            ValidateFullName ();
            ValidateLanguageId ();
            ValidateTimezoneId ();
            ValidateLocationId ();
        }
    }
}