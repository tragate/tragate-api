using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class RegisterFastNewCompanyCommandValidation : UserValidation<RegisterFastNewCompanyCommand> {
        public RegisterFastNewCompanyCommandValidation () {
            ValidateFullName ();
            ValidateBusinessType();
            ValidateLocationId();
            ValidateCountryId();
            ValidateStateId();
        }
    }
}