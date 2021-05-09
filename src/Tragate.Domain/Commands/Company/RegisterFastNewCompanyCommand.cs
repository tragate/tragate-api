using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class RegisterFastNewCompanyCommand : UserCompanyCommand {
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public int CategoryId { get; set; }

        public override bool IsValid () {
            ValidationResult = new RegisterFastNewCompanyCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}