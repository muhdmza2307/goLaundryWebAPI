using goLaundryWebAPI.Helpers;
using goLaundryWebAPI.Models;
using goLaundryWebAPI.Models.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Data
{
    public class CompanyDataLayer
    {
        private static SqlHelper _laundryDBSqlHelper = new SqlHelper("");
        public CompanyDataLayer(IConfiguration iConfiguration)
        {
            _laundryDBSqlHelper = new SqlHelper(iConfiguration.GetConnectionString("laundryDb").ToString());
        }
        public DataTable GetCompanyList()
        {
            SqlParameter[] SQLParam = new SqlParameter[] {};

            DataTable dtCompList = _laundryDBSqlHelper.SqlExecute_DataTable("sp_Get_CompList", SQLParam);

            return dtCompList;
        }
        public DataSet GetCompanyDetails(int Id)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@iCOID", Id)
            };

            DataSet dsCompDetails = _laundryDBSqlHelper.SqlExecute_DataSet("sp_Get_CompById", SQLParam);

            dsCompDetails.Tables[0].TableName = "compInfo";
            dsCompDetails.Tables[1].TableName = "compChildList";
            dsCompDetails.Tables[2].TableName = "compUserList";

            return dsCompDetails;
        }

        public string CreateEditCompany(CompDetailsRequest compDetails)
        {
            string result = "";
            SqlParameter[] queryParams = {
                    new SqlParameter("@iMODE", compDetails.mode),
                    new SqlParameter("@iCOID", compDetails.compId),
                    new SqlParameter("@vaCONAME", compDetails.compName),
                    new SqlParameter("@siPARENT", compDetails.isParent),
                    new SqlParameter("@iPCOID", compDetails.pCompId),
                    new SqlParameter("@vaEMAIL", compDetails.email),
                    new SqlParameter("@vaTELNO", compDetails.telNo),
                    new SqlParameter("@vaFAXNO", compDetails.faxNo),
                    new SqlParameter("@vaADD1", compDetails.add1),
                    new SqlParameter("@vaADD2", compDetails.add2),
                    new SqlParameter("@vaADD3", compDetails.add3),
                    new SqlParameter("@siSTATUS", compDetails.isActive),
                    new SqlParameter("@vaBRANCH", compDetails.branchName)
                };

            result = _laundryDBSqlHelper.SqlExecuteScalar("sp_CreateEdit_CompById", queryParams);

            return result;
        }
    }
}
