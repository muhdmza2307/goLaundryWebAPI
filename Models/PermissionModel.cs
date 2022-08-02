using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class PermissionModel
    {
        public int permId { get; set; }
        public string permCode { get; set; }
        public string permName { get; set; }
        public int isChecked { get; set; }
    }

}
