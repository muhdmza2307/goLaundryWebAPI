using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class RespModel<T>
    {
        public RespModel()
        {
            meta = new Meta();
            data = Activator.CreateInstance<T>();
        }

        public Meta meta { get; set; }

        public T data { get; set; }

        public RespModel<T> Error (string code, string message)
        {
            LogManager.GetCurrentClassLogger().Error(message);
            this.meta.RespCode = code;
            this.meta.RespMessage = message;
            this.data = default(T);
            return this;
        }
    }

    public class Meta
    {
        public Meta()
        {
            RespCode = Convert.ToString(Convert.ToInt32(HttpStatusCode.OK));
            RespMessage = "Success";
        }

        public string RespCode { get; set; }

        public string RespMessage { get; set; }
    }

    public class SuccessResp
    {
        public string msg { get; set; }
    }
}
