using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class ChangePasswordCommanValidation : UserValidation<ChangePasswordCommand> {
        public ChangePasswordCommanValidation () {
            RuleFor (c => c)
                .Custom ((c, context) => {
                    if (c.NewPassword != c.ConfirmPassword) {
                        context.AddFailure ("NewPassword", "passwords doesn't match");
                    }
                });

            RuleFor (c => c.Id)
                .NotEmpty ().WithMessage ("Please ensure you have entered the UserId");

            RuleFor (c => c.OldPassword)
                .NotEmpty ().WithMessage ("Please ensure you have entered the OldPassword");

            RuleFor (c => c.NewPassword)
                .NotEmpty ().WithMessage ("Please ensure you have entered the NewPassword");

            RuleFor (c => c.ConfirmPassword)
                .NotEmpty ().WithMessage ("Please ensure you have entered the ConfirmPassword");
        }
    }
}