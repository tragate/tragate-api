using MediatR;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Events.Image;

namespace Tragate.Domain.EventHandlers
{
    public class ImageEventHandler : INotificationHandler<ImageDeletedEvent>
    {
        private readonly IFileUploadService _fileUploadService;

        public ImageEventHandler(IFileUploadService fileUploadService){
            _fileUploadService = fileUploadService;
        }

        public void Handle(ImageDeletedEvent message){
            _fileUploadService.Delete(message.BucketName, message.Key);
        }
    }
}