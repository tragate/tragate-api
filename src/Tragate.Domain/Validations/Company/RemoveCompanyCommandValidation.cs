using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class RemoveCompanyCommandValidation : UserValidation<RemoveCompanyCommand> {
        public RemoveCompanyCommandValidation () {
            ValidateId ();
        }
    }
}