using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Tragate.Common.Library.Enum;

namespace Tragate.Application.ViewModels {
    public class CategoryViewModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int StatusId { get; set; }
        public string Metakeyword { get; set; }
        public string MetaDescription { get; set; }
        public int CreatedUserId { get; set; }
        public int Priority { get; set; }
        public bool HasChild { get; set; }
    }
}