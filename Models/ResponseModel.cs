using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Models
{
    public class ResponseModel
    {
        public int type { get; set; }
        public object data { get; set; }
        public bool show { get; set; }
        public ResponseModel()
        {
            data = new DataTable();
            show = true;
        }
        public void setResponse(int typeValue, object dataValue, bool showValue)
        {
            type = typeValue;
            data = dataValue;
            show = showValue;
        }
    }

    public class ResponseModel2
    {
        public int type { get; set; }
        public string data { get; set; }
        public bool show { get; set; }
        public ResponseModel2()
        {
            data = "";
            show = true;
        }
        public void setResponse(int typeValue, string dataValue, bool showValue)
        {
            type = typeValue;
            data = dataValue;
            show = showValue;
        }
    }
}
