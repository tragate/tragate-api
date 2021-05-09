using System;
using System.Linq;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;

namespace Tragate.Application {
    public interface IContentService : IDisposable {
        IQueryable<ContentDto> GetAll ();
        ContentDto GetById (int id);
        ContentDto GetBySlug (string slug, int statusId);
        void AddContent (ContentViewModel model);
        void UpdateContent (ContentViewModel model);
    }
}