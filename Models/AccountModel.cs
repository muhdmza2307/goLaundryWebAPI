using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class UserList
    {
        public string userName { get; set; }
        public int userId { get; set; }
        public string compName { get; set; }
        public string email { get; set; }
        public string telNo { get; set; }
        public bool isActive { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
    }

    public class UserDetails : UserList
    {       
        public int compId { get; set; }      
        public string fullName { get; set; }     
        public string token { get; set; }
        public DateTime tokenExpires { get; set; }

        public List<UserPermission> permList { get; set; }
    }

    public class UserPermission
    {
        public int permId { get; set; }
        public string permCode { get; set; }
        public string permName { get; set; }
        public bool isChecked { get; set; }
    }
}
