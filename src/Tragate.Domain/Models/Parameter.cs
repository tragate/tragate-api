using System;
using Tragate.Domain.Core.Models;

namespace Tragate.Domain.Models {
    public class Parameter : Entity {
        public string ParameterGroupName { get; set; }
        public string ParameterType { get; set; }
        public byte ParameterCode { get; set; }
        public string ParameterValue1 { get; set; }
        public string ParameterValue2 { get; set; }
        public string ParameterValue3 { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}