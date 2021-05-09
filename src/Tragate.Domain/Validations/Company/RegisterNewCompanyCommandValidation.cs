using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class RegisterNewCompanyCommandValidation : UserValidation<RegisterNewCompanyCommand> {
        public RegisterNewCompanyCommandValidation () {
            ValidateFullName ();
            ValidatePerson ();
            ValidateBusinessType ();
            ValidateLocationId ();
            ValidateCountryId ();
            ValidateStateId ();
        }
    }
}