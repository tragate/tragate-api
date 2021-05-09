using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class AddNewCategoryCommandValidation : CategoryValidation<AddNewCategoryCommand>
    {
        public AddNewCategoryCommandValidation()
        {
            ValidateTitle();
            ValidateStatusId();
            ValitdateEqualIdAndParentId();
        }
    }
}