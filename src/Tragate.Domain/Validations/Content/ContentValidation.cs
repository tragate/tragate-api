using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public abstract class ContentValidation<T> : AbstractValidator<T> where T : ContentCommand {
        protected void ValidateTitle () {
            RuleFor (c => c.Title)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Title");
        }

        protected void ValidateBody () {
            RuleFor (c => c.Body)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Body");
        }

        protected void ValidateDescription () {
            RuleFor (c => c.Description)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Description");
        }

        protected void ValidateId () {
            RuleFor (c => c.Id)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Id");
        }

        protected void ValidateCreatedUserId () {
            RuleFor (c => c.CreatedUserId)
                .NotEmpty ().WithMessage ("Please ensure you have entered the CreatedUserId");
        }

        protected void ValidateStatusId () {
            RuleFor (c => c)
                .Custom ((c, context) => {
                    if (c.StatusType == StatusType.All) {
                        context.AddFailure ("StatusType", "Status shouldn't be 0");
                    }
                });
        }
    }
}