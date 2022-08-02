using goLaundryWebAPI.Data;
using goLaundryWebAPI.Helpers;
using goLaundryWebAPI.Models;
using goLaundryWebAPI.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

        [HttpPost("GetUserList")]
        public IActionResult GetUserList()
        {
            RespModel<List<UserList>> resp = new RespModel<List<UserList>>();

            try
            {
                DataTable dtResult = new UserDataLayer(_iConfiguration).GetUserList();

                List<UserList> compList = new List<UserList>();
                compList = CommonHelper.ConvertDataTableToListObject<UserList>(dtResult);

                resp.data = compList;

                return Ok(resp);
            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
        }

        [HttpPost("GetUserById")]
        public IActionResult GetUserById([FromBody] UserRequest request)
        {
            RespModel<UserDetails> resp = new RespModel<UserDetails>();

            try
            {
                DataSet dsResult = new UserDataLayer(_iConfiguration).GetUserDetails(request.userId);

                if (dsResult.Tables[0].Rows.Count < 1)
                {
                    return Ok(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.OK)), "No record found."));
                }
                else
                {
                    UserDetails usrDtls = new UserDetails();
                    usrDtls = CommonHelper.GetObjectItem<UserDetails>(dsResult.Tables[0].Rows[0]);

                    usrDtls.permList = new List<UserPermission>();
                    if ((dsResult != null && dsResult.Tables.Count > 0) ? dsResult.Tables[1].Rows.Count > 0 : false)
                    {
                        usrDtls.permList = CommonHelper.ConvertDataTableToListObject<UserPermission>(dsResult.Tables[1]);
                    }

                    resp.data = usrDtls;

                    return Ok(resp);
                }

            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
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
