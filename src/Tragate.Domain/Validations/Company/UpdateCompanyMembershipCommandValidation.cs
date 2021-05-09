using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdateCompanyMembershipCommandValidation : UserValidation<UpdateCompanyMembershipCommand>
    {
        public UpdateCompanyMembershipCommandValidation(){
            ValidateMembershipTypeId();
            ValidateMembershipPackageId();
        }
    }
}