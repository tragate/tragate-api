using System.Collections.Generic;

namespace Tragate.Common.Result
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public List<Link> Links { get; set; }
    }
}