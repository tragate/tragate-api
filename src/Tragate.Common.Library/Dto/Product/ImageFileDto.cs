using Microsoft.AspNetCore.Http;

namespace Tragate.Common.Library.Dto
{
    public class ImageFileDto
    {
        public IFormFile File { get; set; }
        public string Key { get; set; }
        public string ContentType { get; set; }
    }
}