using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdateCategoryCommandValidation : CategoryValidation<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidation()
        {
            ValidateId();
            ValidateTitle();
            ValidateStatusId();
            ValidateCreatedUserId();
            ValitdateEqualIdAndParentId();
        }
    }
}