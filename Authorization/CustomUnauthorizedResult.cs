using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Authorization
{
    public class CustomUnauthorizedResult : JsonResult
    {
        //public CustomUnauthorizedResult(string message)
        //    : base(new CustomError(message))
        //{
        //    StatusCode = StatusCodes.Status401Unauthorized;
        //}

        //public CustomUnauthorizedResult(ResponseModel2 resp)
        //    : base(new CustomError(resp))
        //{
        //    StatusCode = StatusCodes.Status401Unauthorized;
        //}

        public CustomUnauthorizedResult(RespModel<object> resp)
            : base(new CustomError(resp))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
