using goLaundryWebAPI.Helpers;
using goLaundryWebAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Data
{
    public class UserDataLayer
    {
        private static SqlHelper _laundryDBSqlHelper = new SqlHelper("");
        public UserDataLayer(IConfiguration iConfiguration)
        {
            _laundryDBSqlHelper = new SqlHelper(iConfiguration.GetConnectionString("laundryDb").ToString());
        }
        public DataTable GetUserList()
        {
            SqlParameter[] SQLParam = new SqlParameter[] { };

            DataTable dtUserList = _laundryDBSqlHelper.SqlExecute_DataTable("sp_Get_UserList", SQLParam);

            return dtUserList;
        }
        public DataSet GetUserDetails(int Id)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@iUSID", Id)
            };

            DataSet dsCompDetails = _laundryDBSqlHelper.SqlExecute_DataSet("sp_Get_UserById", SQLParam);

            return dsCompDetails;
        }
        public DataTable GetPermissionByUserId(int Id)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@iMODE", 1),
                new SqlParameter("@iUSID", Id)
            };

            DataTable dtUserPermList = _laundryDBSqlHelper.SqlExecute_DataTable("sp_Get_PermByUserId", SQLParam);

            return dtUserPermList;
        }
        public string CreateEditUser(UserModel userDetails)
        {
            string result = "";
            SqlParameter[] queryParams = {
                    new SqlParameter("@iMODE", userDetails.mode),
                    new SqlParameter("@iUSID", userDetails.userId),
                    new SqlParameter("@iCOID", userDetails.compId),
                    new SqlParameter("@vaUSNAME", userDetails.userName),
                    new SqlParameter("@vaFULLNAME", userDetails.fullName),
                    new SqlParameter("@vaEMAIL", userDetails.email),
                    new SqlParameter("@vaTELNO", userDetails.telNo),
                    new SqlParameter("@siSTATUS", userDetails.isActive),
                    new SqlParameter("@vaHASHPASS", userDetails.password),
                    new SqlParameter("@vaPERMID", userDetails.strListPermId)
                };

            result = _laundryDBSqlHelper.SqlExecuteScalar("sp_CreateEdit_UserById", queryParams);

            return result;
        }

        public DataSet GetUserMenuPermission(int Id)
        {
            SqlParameter[] SQLParam = new SqlParameter[] {
                new SqlParameter("@iUSID", Id)
            };

            DataSet dsUserMenuPermList = _laundryDBSqlHelper.SqlExecute_DataSet("sp_Get_UserMenuPemissionById", SQLParam);

            dsUserMenuPermList.Tables[0].TableName = "parentMenuList";
            dsUserMenuPermList.Tables[1].TableName = "childMenuList";

            return dsUserMenuPermList;
        }
    }
}
