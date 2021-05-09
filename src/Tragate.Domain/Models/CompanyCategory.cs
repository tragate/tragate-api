using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models
{
    public class CompanyCategory : Entity
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
    }
}