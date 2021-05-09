using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class ChangePasswordCommand : UserCompanyCommand {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public override bool IsValid () {
            ValidationResult = new ChangePasswordCommanValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}