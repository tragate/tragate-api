using System.IO;
using FluentValidation;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UploadImageCommandValidation : UserValidation<UploadImageCommand>
    {
        public UploadImageCommandValidation(){
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

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please ensure you have entered the UserId");
        }
    }
}