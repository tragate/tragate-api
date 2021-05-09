using System;
using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class CompanyTaskValidation<T> : AbstractValidator<T> where T : CompanyTaskCommand
    {
        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateCompanyId(){
            RuleFor(c => c.CompanyId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateDescription(){
            RuleFor(c => c.Description)
                .NotEmpty();
        }

        protected void ValidateResponsibleUserId(){
            RuleFor(c => c.ResponsibleUserId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateCompanyTaskTypeId(){
            RuleFor(c => c.CompanyTaskTypeId)
                .NotEmpty()
                .Must(x => x.Equals(CompanyTaskType.UpdateCompanyInfo) ||
                           x.Equals(CompanyTaskType.UpdateCompanyProducts));
        }

        protected void ValidateStatusTypeId(){
            RuleFor(c => c.StatusId)
                .NotEmpty()
                .Must(x =>
                    x.Equals(StatusType.New) ||
                    x.Equals(StatusType.WaitingApprove) ||
                    x.Equals(StatusType.Active) ||
                    x.Equals(StatusType.Deleted) ||
                    x.Equals(StatusType.Passive) ||
                    x.Equals(StatusType.Transferred) ||
                    x.Equals(StatusType.Completed));
        }

        protected void ValidateEndDate(){
            RuleFor(c => c.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today);
        }

        protected void ValidateUpdatedUserId(){
            RuleFor(c => c.UpdatedUserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}