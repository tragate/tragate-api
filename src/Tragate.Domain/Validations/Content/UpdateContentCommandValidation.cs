using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateContentCommandValidation : ContentValidation<UpdateContentCommand> {
        public UpdateContentCommandValidation () {
            ValidateId ();
            ValidateTitle ();
            ValidateBody ();
            ValidateDescription ();
            ValidateStatusId ();
            ValidateCreatedUserId ();
        }
    }
}