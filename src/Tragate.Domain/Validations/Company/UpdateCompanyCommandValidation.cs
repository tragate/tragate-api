using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateCompanyCommandValidation : UserValidation<UpdateCompanyCommand> {
        public UpdateCompanyCommandValidation () {
            ValidateFullName ();
            ValidateId ();
            ValidateStatusId();
        }
    }
}