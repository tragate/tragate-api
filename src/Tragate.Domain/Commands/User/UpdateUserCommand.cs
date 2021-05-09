using System;
using Microsoft.AspNetCore.Http;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateUserCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new UpdateUserCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}