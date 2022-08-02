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
    public class ServiceDataLayer
    {
        private static SqlHelper _laundryDBSqlHelper = new SqlHelper("");
        public ServiceDataLayer(IConfiguration iConfiguration)
        {
            _laundryDBSqlHelper = new SqlHelper(iConfiguration.GetConnectionString("laundryDb").ToString());
        }
        public DataTable GetServiceList()
        {
            SqlParameter[] SQLParam = new SqlParameter[] { };

            DataTable dtSvcList = _laundryDBSqlHelper.SqlExecute_DataTable("sp_Get_ServiceList", SQLParam);

            return dtSvcList;
        }
        public DataSet GetServiceDetails(int Id)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@iSVCID", Id)
            };

            DataSet dsSvcDetails = _laundryDBSqlHelper.SqlExecute_DataSet("sp_Get_ServiceById", SQLParam);

            return dsSvcDetails;
        }
    }
}
