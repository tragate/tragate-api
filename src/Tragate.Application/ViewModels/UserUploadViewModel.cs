using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Tragate.Application.ViewModels {
    public class UserUploadViewModel {
        public int UserId { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}