using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.CompanyMembership
{
    public class AddNewCompanyMembershipCommandValidation : CompanyMembershipValidation<AddNewCompanyMembershipCommand>
    {
        public AddNewCompanyMembershipCommandValidation(){
            ValidateCompanyId();
            ValidateMembershipPackageId();
            ValidateMembershipTypeId();
            ValidateStartDate();
            ValidateEndDate();
            ValidateDates();
            ValidateCreatedUserId();
        }
    }
}