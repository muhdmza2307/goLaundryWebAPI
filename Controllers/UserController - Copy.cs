using goLaundryWebAPI.Data;
using goLaundryWebAPI.Helpers;
using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public UserController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            ResponseModel2 resp = new ResponseModel2();

            try
            {
                DataTable dtResult = new UserDataLayer(_iConfiguration).GetUserList();
                string data = JsonConvert.SerializeObject(dtResult);
                resp.setResponse(1, data, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
            }

            return Ok(resp);
        }

        [HttpGet("GetUserById")]
        public IActionResult GetUserById([FromQuery] int Id)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                DataSet dsResult = new UserDataLayer(_iConfiguration).GetUserDetails(Id);
                UserModel userDetails = new UserModel();
                userDetails.userId = Convert.ToInt32(dsResult.Tables[0].Rows[0]["iUSID"]);
                userDetails.compId = Convert.ToInt32(dsResult.Tables[0].Rows[0]["iCOID"]);
                userDetails.userName = dsResult.Tables[0].Rows[0]["vaUSNAME"].ToString();
                userDetails.fullName = dsResult.Tables[0].Rows[0]["vaFULLNAME"].ToString();
                userDetails.email = dsResult.Tables[0].Rows[0]["vaEMAIL"].ToString();
                userDetails.telNo = dsResult.Tables[0].Rows[0]["vaTELNO"].ToString();
                userDetails.isActive = (bool)dsResult.Tables[0].Rows[0]["siSTATUS"];
                userDetails.permList = new List<PermissionModel>();

                if ((dsResult != null && dsResult.Tables.Count > 0) ? dsResult.Tables[1].Rows.Count > 0 : false)
                {
                    for (int i = 0; i < dsResult.Tables[1].Rows.Count; i++)
                    {
                        PermissionModel permDetails = new PermissionModel();
                        permDetails.permId = Convert.ToInt32(dsResult.Tables[1].Rows[i]["iPERM"]);
                        permDetails.permCode = dsResult.Tables[1].Rows[i]["vaPERMCODE"].ToString();
                        permDetails.permName = dsResult.Tables[1].Rows[i]["vaPERMNAME"].ToString();
                        permDetails.isChecked = Convert.ToInt32(dsResult.Tables[1].Rows[i]["isCHECKED"]);

                        userDetails.permList.Add(permDetails);
                    }
                }

                resp.setResponse(1, new { userDetails }, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
                return Unauthorized(resp);
            }


            return Ok(resp);
        }

        [HttpGet("GetPermissionByUserId")]
        public IActionResult GetPermissionByUserId([FromQuery] int Id)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                DataTable dtResult = new UserDataLayer(_iConfiguration).GetPermissionByUserId(Id);                
                string strListPermId = dtResult.Rows[0]["vaPERMID"].ToString();

                resp.setResponse(1, strListPermId, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
                return Unauthorized(resp);
            }


            return Ok(resp);
        }

        [HttpPost("CreateEditUser")]
        public IActionResult CreateEditUser([FromBody] UserModel userDetails)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                CommonHelper _commonHelper = new CommonHelper();
                string hPass = _commonHelper.hashPass(userDetails.userName, userDetails.password);
                userDetails.password = hPass;
                string result = new UserDataLayer(_iConfiguration).CreateEditUser(userDetails);
                resp.setResponse(1, result, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
                return Unauthorized(resp);
            }

            return Ok(resp);
        }

        [HttpGet("GetUserMenuPermission")]
        public IActionResult GetUserMenuPermission([FromQuery] int Id)
        {
            ResponseModel2 resp = new ResponseModel2();

            try
            {
                DataSet dtResult = new UserDataLayer(_iConfiguration).GetUserMenuPermission(Id);
                string data = JsonConvert.SerializeObject(dtResult);
                resp.setResponse(1, data, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
                return Unauthorized(resp);
            }


            return Ok(resp);
        }

    }
}
