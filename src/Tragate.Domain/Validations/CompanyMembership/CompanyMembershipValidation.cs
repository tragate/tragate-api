using System;
using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.CompanyMembership
{
    public class CompanyMembershipValidation<T> : AbstractValidator<T> where T : CompanyMembershipCommand
    {
        protected void ValidateCompanyId(){
            RuleFor(c => c.CompanyId)
                .NotEmpty();
        }

        protected void ValidateMembershipPackageId(){
            RuleFor(c => c.MembershipPackageId)
                .GreaterThanOrEqualTo(0);
        }

        protected void ValidateMembershipTypeId(){
            RuleFor(c => c.MembershipTypeId)
                .GreaterThanOrEqualTo(0);
        }

        protected void ValidateStartDate(){
            RuleFor(c => c.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today);
        }

        protected void ValidateEndDate(){
            RuleFor(c => c.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today);
        }

        protected void ValidateDates(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.StartDate > c.EndDate){
                        context.AddFailure("StartDate", "Startdate should not bigger than enddate");
                    }
                });
        }


        protected void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty();
        }
    }
}