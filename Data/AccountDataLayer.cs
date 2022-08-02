using goLaundryWebAPI.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Data
{
    public class AccountDataLayer
    {
        private static SqlHelper _laundryDBSqlHelper = new SqlHelper("");
        public AccountDataLayer(IConfiguration iConfiguration)
        {
            _laundryDBSqlHelper = new SqlHelper(iConfiguration.GetConnectionString("laundryDb").ToString());
        }
        public DataTable GetAccountLogin(string userName, string hPass)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@vaUSNAME", userName),
                new SqlParameter("@vaPASS", hPass)
            };

            DataTable dtCompList = _laundryDBSqlHelper.SqlExecute_DataTable("sp_Get_UserLogin", SQLParam);

            return dtCompList;
        }

    }
}
