using goLaundryWebAPI.Data;
using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using goLaundryWebAPI.Helpers;
using Newtonsoft.Json;
using System.Net;
using goLaundryWebAPI.Models.Request;
using goLaundryWebAPI.Models.Response;

namespace goLaundryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public AccountController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AccRequest loginPass)
        {
            RespModel<AccDetails> resp = new RespModel<AccDetails>();

            try
            {
                DataTable dtResult = new AccountDataLayer(_iConfiguration).GetAccountLogin(loginPass.userName, loginPass.password);

                if (dtResult.Rows.Count < 1)
                {
                    return Ok(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.OK)), "Bad Login."));
                }
                else
                {
                    AccDetails accDetails = new AccDetails();
                    accDetails = CommonHelper.GetObjectItem<AccDetails>(dtResult.Rows[0]);

                    //JWT Tokens
                    var now = DateTime.UtcNow;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = Encoding.ASCII.GetBytes(_iConfiguration["JWTSettings:Key"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                            new Claim(ClaimTypes.Name, accDetails.userId.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(Double.Parse(_iConfiguration["JWTSettings:ExpMinute"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    accDetails.token = tokenHandler.WriteToken(token);
                    accDetails.tokenExpires = now.AddMinutes(Double.Parse(_iConfiguration["JWTSettings:ExpMinute"]));

                    resp.data = accDetails;
                    return Ok(resp);
                }

            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
        }

        [HttpPost("GetAccMenuPermission")]
        public IActionResult GetAccMenuPermission([FromBody] AccPermRequest req)
        {
            RespModel<MenuList> resp = new RespModel<MenuList>();

            try
            {
                DataSet dsResult = new UserDataLayer(_iConfiguration).GetUserMenuPermission(req.userId);

                if (dsResult.Tables[0].Rows.Count < 1)
                {
                    return Ok(resp.Error(Convert.ToString(Convert.ToUInt32(HttpStatusCode.OK)), "No Record Found"));
                }
                else
                {
                    MenuList menuList = new MenuList();
                    menuList.parentMenuList = CommonHelper.ConvertDataTableToListObject<AccMenu>(dsResult.Tables[0]);
                    menuList.childMenuList = CommonHelper.ConvertDataTableToListObject<AccMenu>(dsResult.Tables[1]);

                    resp.data = menuList;

                    return Ok(resp);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToUInt32(HttpStatusCode.BadRequest)), ex.Message.ToString()));
            }

        }
    }
}
