using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models.Request
{
    public class CompRequest
    {
        public int compId { get; set; }
    }

    public class CompDetailsRequest : CompRequest
    {
        public string compName { get; set; }
        public bool isParent { get; set; }
        public bool isActive { get; set; }
        public string branchName { get; set; }
        public int? pCompId { get; set; }
        public string email { get; set; }
        public string telNo { get; set; }
        public string faxNo { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public int mode { get; set; }
    }
}
