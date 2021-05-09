using System.IO;
using System.Linq;
using FluentValidation;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.ProductImage
{
    public abstract class ProductImageValidation<T> : AbstractValidator<T> where T : ProductImageCommand
    {
        protected void ValidateId(){
            RuleFor(c => c.Id).GreaterThan(0);
        }

        protected void ValidateFiles(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (!c.Files.Any()){
                        context.AddFailure("Files", "Files doesn't exists");
                    }

                    foreach (var file in c.Files){
                        if (!Path.GetExtension(file.FileName).CheckFileExtensions()){
                            context.AddFailure("Files",
                                "File extensions should be one of '.jpg','.jpeg','.png','.bmp','.gif'");
                        }
                    }
                });
        }

        protected void ValidateFileCount(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.Files.Count > 10){
                        context.AddFailure("Files", "Files shouldn't be greater than 10");
                    }
                });
        }

        protected void ValidateUuId(){
            RuleFor(c => c.UuId)
                .NotEmpty().WithMessage("Please ensure you have entered the UuId");
        }
    }
}