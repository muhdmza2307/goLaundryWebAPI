using goLaundryWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Authorization
{
    //public class CustomError : ResponseModel2
    //{

    //    public CustomError(ResponseModel2 rs)
    //    {
    //        type = rs.type;
    //        data = rs.data;
    //        show = rs.show;
    //    }
    //}

    public class CustomError : RespModel<object>
    {

        public CustomError(RespModel<object> rs)
        {
            meta.RespCode = rs.meta.RespCode;
            meta.RespMessage = rs.meta.RespMessage;
        }
    }
}
