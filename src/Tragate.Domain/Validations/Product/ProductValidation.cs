using System.Data;
using System.Linq;
using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public abstract class ProductValidation<T> : AbstractValidator<T> where T : ProductCommand
    {
        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Please ensure you have entered the Id");
        }

        protected void ValidateUuId(){
            RuleFor(c => c.UuId)
                .NotEmpty().WithMessage("Please ensure you have entered the UuId");
        }

        protected void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty().WithMessage("Please ensure you have entered the CreatedUserId")
                .GreaterThan(0);
        }

        protected void ValidateUpdatedUserId(){
            RuleFor(c => c.UpdatedUserId)
                .NotEmpty().WithMessage("Please ensure you have entered the UpdatedUserId")
                .GreaterThan(0);
        }

        protected void ValidateListImagePath(){
            RuleFor(c => c.ListImagePath)
                .NotEmpty().WithMessage("Please ensure you have entered the ListImagePath");
        }

        protected void ValidateCompanyId(){
            RuleFor(c => c.CompanyId)
                .NotEmpty().WithMessage("Please ensure you have entered the CompanyId")
                .GreaterThan(0);
        }

        protected void ValidateTitle(){
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title");
        }

        protected void ValidatePriceLow(){
            RuleFor(c => c.PriceLow)
                .GreaterThanOrEqualTo(0.0M);
        }

        protected void ValidatePriceHigh(){
            RuleFor(c => c.PriceHigh)
                .GreaterThanOrEqualTo(0.0M);
        }

        protected void ValidateCurrencyId(){
            RuleFor(c => c.CurrencyId)
                .NotEmpty().WithMessage("Please ensure you have entered the CurrencyId")
                .GreaterThan(0);
        }

        protected void ValidateUnitTypeId(){
            RuleFor(c => c.UnitTypeId)
                .NotEmpty().WithMessage("Please ensure you have entered the UnitTypeId")
                .GreaterThan(0);
        }

        protected void ValidateOriginLocationId(){
            RuleFor(c => c.OriginLocationId)
                .NotEmpty().WithMessage("Please ensure you have entered the OriginLocationId")
                .GreaterThan(0);
        }

        protected void ValidateCategoryId(){
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("Please ensure you have entered the CategoryId")
                .GreaterThan(0);
        }

        protected void ValidateStatusId(){
            RuleFor(c => c.StatusId)
                .NotEmpty().WithMessage("Please ensure you have entered the StatusId")
                .GreaterThan((byte) 0);
        }
    }
}