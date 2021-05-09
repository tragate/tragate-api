using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class UpdateStatusProductCommandValidation : ProductValidation<UpdateStatusProductCommand>
    {
        public UpdateStatusProductCommandValidation(){
            ValidateId();
            ValidateStatusId();
            ValidateUpdatedUserId();
        }
    }
}