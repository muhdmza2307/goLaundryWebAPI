using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models.Request
{
    public class AccRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class AccPermRequest
    {
        public int userId { get; set; }
    }
}
