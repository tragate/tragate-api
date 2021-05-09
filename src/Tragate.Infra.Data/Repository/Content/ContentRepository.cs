using System.Linq;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data {
    public class ContentRepository : Repository<Content>, IContentRepository {
        public ContentRepository (TragateContext context) : base (context) {

        }

        public Content GetBySlug (string slug, int statusId) {
            return Db.Content.FirstOrDefault (x => x.Slug == slug && x.StatusId == statusId);
        }
    }
}