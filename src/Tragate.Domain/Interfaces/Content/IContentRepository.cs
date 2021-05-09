using System.Linq;
using Tragate.Domain.Models;

namespace Tragate.Domain.Interfaces {
    public interface IContentRepository : IRepository<Content> {
        Content GetBySlug (string slug, int statusId);
    }
}