using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UploadCategoryImageCommandValidation : CategoryValidation<UploadCategoryImageCommand> {
        public UploadCategoryImageCommandValidation () {
            ValidateId ();
            ValidateImage ();
        }
    }
}