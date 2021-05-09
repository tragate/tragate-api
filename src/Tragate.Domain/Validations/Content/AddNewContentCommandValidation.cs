using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class AddNewContentCommandValidation : ContentValidation<AddNewContentCommand> {
        public AddNewContentCommandValidation () {
            ValidateTitle ();
            ValidateBody ();
            ValidateDescription ();
            ValidateStatusId (); 
            ValidateCreatedUserId ();
        }
    }
}