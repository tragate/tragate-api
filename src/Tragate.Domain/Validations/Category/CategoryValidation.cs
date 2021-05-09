using System.IO;
using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public abstract class CategoryValidation<T> : AbstractValidator<T> where T : CategoryCommand
    {
        protected void ValidateTitle(){
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title");
        }

        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Please ensure you have entered the Id");
        }

        protected void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty().WithMessage("Please ensure you have entered the CreatedUserId");
        }

        protected void ValidateStatusId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.StatusType == StatusType.All){
                        context.AddFailure("StatusType", "Status shouldn't be 0");
                    }
                });
        }

        protected void ValitdateEqualIdAndParentId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.ParentId.HasValue){
                        if (c.Id == c.ParentId.Value)
                            context.AddFailure("ParentId", "The root of category shouldn't be itself");
                    }
                });
        }

        protected void ValidateImage(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.UploadedFile == null){
                        context.AddFailure("UploadedFile", "File doesn't exists");
                    }
                    else if (!Path.GetExtension(c.UploadedFile.FileName).CheckFileExtensions()){
                        context.AddFailure("UploadedFile",
                            "File extensions should be one of '.jpg','.jpeg','.png','.bmp','.gif'");
                    }
                });
        }
    }
}