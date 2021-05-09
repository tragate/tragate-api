using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tragate.Common.Result
{
    public class BadResult : BaseResult
    {
        public IEnumerable<string> Errors { get; set; }

        public override string ToString(){
            return JsonConvert.SerializeObject(this);
        }
    }
}