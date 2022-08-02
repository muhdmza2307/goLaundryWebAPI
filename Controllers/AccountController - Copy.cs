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

namespace goLaundryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public AccountController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginDetails)
        {
            ResponseModel2 resp = new ResponseModel2();

            try
            {
                CommonHelper _commonHelper = new CommonHelper();
                //string hPass = _commonHelper.hashPass(loginDetails.userName, loginDetails.password);
                DataTable dtResult = new AccountDataLayer(_iConfiguration).GetAccountLogin(loginDetails.userName, loginDetails.password);

                if (dtResult.Rows.Count > 0)
                {
                    string userEmail = dtResult.Rows[0]["vaEMAIL"].ToString();

                    //JWT Tokens
                    var now = DateTime.UtcNow;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = Encoding.ASCII.GetBytes(_iConfiguration["JWTSettings:Key"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                            new Claim(ClaimTypes.Email, userEmail)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(Double.Parse(_iConfiguration["JWTSettings:ExpMinute"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    dtResult.Columns.Add("token", typeof(System.String));
                    dtResult.Columns.Add("tokenExpires", typeof(System.DateTime));
                    dtResult.Rows[0]["token"] = tokenHandler.WriteToken(token);
                    dtResult.Rows[0]["tokenExpires"] = now.AddMinutes(Double.Parse(_iConfiguration["JWTSettings:ExpMinute"]));

                    string data = JsonConvert.SerializeObject(dtResult);
                    resp.setResponse(1, data, false);
                }
                else
                {
                    resp.setResponse(0, "Bad Login", true);
                }


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
