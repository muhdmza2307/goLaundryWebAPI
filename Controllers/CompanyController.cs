using goLaundryWebAPI.Data;
using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using goLaundryWebAPI.Helpers;
using goLaundryWebAPI.Extension;
using goLaundryWebAPI.Models.Request;

namespace goLaundryWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public CompanyController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }


        [HttpPost("GetCompanyList")]
        public IActionResult GetCompanyList()
        {
            RespModel<List<CompanyList>> resp = new RespModel<List<CompanyList>>();

            try
            {
                DataTable dtResult = new CompanyDataLayer(_iConfiguration).GetCompanyList();

                List<CompanyList> compList = new List<CompanyList>();
                compList = CommonHelper.ConvertDataTableToListObject<CompanyList>(dtResult);

                resp.data = compList;

                return Ok(resp);
            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
        }

        [HttpPost("GetCompanyById")]
        public IActionResult GetCompanyById([FromBody] CompRequest request)
        {
            RespModel<CompanyDetails> resp = new RespModel<CompanyDetails>();

            try
            {
                DataSet dsResult = new CompanyDataLayer(_iConfiguration).GetCompanyDetails(request.compId);

                if (dsResult.Tables[0].Rows.Count < 1)
                {
                    return Ok(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.OK)), "No record found."));
                }
                else
                {
                    CompanyDetails compDtls = new CompanyDetails();
                    compDtls = CommonHelper.GetObjectItem<CompanyDetails>(dsResult.Tables[0].Rows[0]);

                    compDtls.childComp = new List<CompanyList>();
                    if ((dsResult != null && dsResult.Tables.Count > 0) ? dsResult.Tables[1].Rows.Count > 0 : false)
                    {
                        compDtls.childComp = CommonHelper.ConvertDataTableToListObject<CompanyList>(dsResult.Tables[1]);
                    }

                    resp.data = compDtls;

                    return Ok(resp);
                }
                
            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
        }


        [HttpPost("CreateEditCompany")]
        public IActionResult CreateEditCompany([FromBody] CompDetailsRequest request)
        {
            RespModel<SuccessResp> resp = new RespModel<SuccessResp>();
            try
            {
                string result = new CompanyDataLayer(_iConfiguration).CreateEditCompany(request);

                if (result.ToUpper().Contains("ERROR"))
                    return Ok(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.OK)), result));
                else
                {
                    SuccessResp sr = new SuccessResp();
                    sr.msg = result;
                    resp.data = sr;
                    return Ok(resp);
                }
                    
            }
            catch (Exception e)
            {
                return BadRequest(resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.BadRequest)), e.Message.ToString()));
            }
        }

    }
}
