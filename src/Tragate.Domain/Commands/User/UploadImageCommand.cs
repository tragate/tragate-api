using System;
using Microsoft.AspNetCore.Http;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UploadImageCommand : UserCompanyCommand {
        public int UserId { get; set; }
        public IFormFile UploadedFile { get; set; }

        public override bool IsValid () {
            ValidationResult = new UploadImageCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}