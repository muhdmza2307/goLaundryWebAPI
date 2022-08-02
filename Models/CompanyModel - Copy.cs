using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class CompanyModel : CommonModel
    {
        public int? compId { get; set; }
        public string compName { get; set; }
        public string branchName { get; set; }
        public bool isParent { get; set; }
        public int? pCompId { get; set; }
        public string email { get; set; }
        public string telNo { get; set; }
        public string faxNo { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public bool isActive { get; set; }

        public string userName { get; set; }
        public string createdDate { get; set; }
        //public List<CompanyModel> childCompList { get; set; }
        //public List<UserModel> userList { get; set; }

    }
}
