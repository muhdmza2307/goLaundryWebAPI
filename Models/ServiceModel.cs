using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class ServiceModel
    {
        public int svcId { get; set; }
        public int compId { get; set; }
        public string svcCode { get; set; }
        public string svcName { get; set; }
        public string svcDesc { get; set; }
        public decimal price { get; set; }
        public string unit { get; set; }
        public bool isActive { get; set; }
    }
}
