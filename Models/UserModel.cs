using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class UserModel : CommonModel 
    {
        public int userId { get; set; }
        public int compId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string telNo { get; set; }
        public bool isActive { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public DateTime tokenExpires { get; set; }
        public List<PermissionModel> permList { get; set; }
        public string strListPermId { get; set; }
    }
}
