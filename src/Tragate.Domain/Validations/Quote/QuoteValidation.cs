using System;
using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public abstract class QuoteValidation<T> : AbstractValidator<T> where T : QuoteCommand
    {
        protected void ValidateId(){
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateTitle(){
            RuleFor(x => x.Title)
                .NotEmpty();
        }

        protected void ValidateDescription(){
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("'User Message' should not be empty.");
        }

        protected void ValidateBuyerUserEmail(){
            RuleFor(x => x.BuyerUserEmail)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.BuyerUserEmail));
        }


        protected void ValidateBuyerUserId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    switch (c.BuyerUserId){
                        case null when !string.IsNullOrEmpty(c.BuyerUserEmail):
                            if (!c.BuyerUserCountryId.HasValue){
                                context.AddFailure("BuyerUserCountryId", "Buyer User CountryId should not be empty");
                            }

                            if (!c.BuyerUserStateId.HasValue){
                                context.AddFailure("BuyerUserStateId", "Buyer User StateId should not be empty");
                            }

                            break;
                        case null:
                            context.AddFailure("BuyerUserEmail", "Buyer User Email should not be empty");
                            break;
                        default:
                            if (c.BuyerUserId <= 0){
                                context.AddFailure("BuyerUserId", "'Buyer User Id' must be greater than '0'.");
                            }

                            break;
                    }
                });
        }


        protected void ValidateBuyerCompanyId(){
            RuleFor(x => x.BuyerCompanyId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateSellerUserId(){
            RuleFor(x => x.SellerUserId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateSellerCompanyId(){
            RuleFor(x => x.SellerCompanyId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateProductPrice(){
            RuleFor(x => x.ProductPrice)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateShippingFee(){
            RuleFor(x => x.ShippingFee)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateInsuranceFee(){
            RuleFor(x => x.InsuranceFee)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateTotalPrice(){
            RuleFor(x => x.TotalPrice)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidatePaid(){
            RuleFor(x => x.Paid)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateBalance(){
            RuleFor(x => x.Balance)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateCurrencyId(){
            RuleFor(x => x.CurrencyId)
                .NotEmpty();
        }

        protected void ValidatePaymentNote(){
            RuleFor(x => x.PaymentNote)
                .NotEmpty();
        }

        protected void ValidateShipmentNote(){
            RuleFor(x => x.ShipmentNote)
                .NotEmpty();
        }

        protected void ValidateShipmentUserAddressId(){
            RuleFor(x => x.ShipmentUserAddressId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateInvoiceUserAddressId(){
            RuleFor(x => x.InvoiceUserAddressId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateTradeTermId(){
            RuleFor(x => x.TradeTermId)
                .NotEmpty();
        }

        protected void ValidateShippingMethodId(){
            RuleFor(x => x.ShippingMethodId)
                .NotEmpty();
        }

        protected void ValidateQuoteStatusId(){
            RuleFor(x => x.QuoteStatusId)
                .NotEmpty();
        }

        protected void ValidateOrderStatusId(){
            RuleFor(x => x.OrderStatusId)
                .NotEmpty();
        }

        protected void ValidateBuyerContactStatusId(){
            RuleFor(x => x.BuyerContactStatusId)
                .NotEmpty();
        }

        protected void ValidateSellerContactStatusId(){
            RuleFor(x => x.SellerContactStatusId)
                .NotEmpty();
        }


        protected void ValidateEmptyBuyerAndSellerContactStatusId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.BuyerContactStatusId == 0 && c.SellerContactStatusId == 0){
                        context.AddFailure("BuyerContactStatusId", "Invalid operation");
                    }
                });
        }

        protected void ValidateFillBuyerAndSellerContactStatusId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.BuyerContactStatusId != 0 && c.SellerContactStatusId != 0){
                        context.AddFailure("BuyerContactStatusId", "Invalid operation");
                    }
                });
        }

        protected void ValidateShipmentDate(){
            RuleFor(x => x.ShipmentDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now);
        }

        protected void ValidateDeliveryDate(){
            RuleFor(x => x.DeliveryDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now);
        }

        protected void ValidateCreatedDate(){
            RuleFor(x => x.CreatedDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now);
        }

        protected void ValidateCreatedUserId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if ((c.BuyerUserId.HasValue || c.SellerUserId.HasValue) && !c.CreatedUserId.HasValue){
                        context.AddFailure("CreatedUserId", "Created User Id should not be empty");
                    }
                });
        }
    }
}