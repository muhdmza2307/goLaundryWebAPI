using goLaundryWebAPI.Data;
using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class ServiceController : ControllerBase
    {
        private readonly IConfiguration _iConfiguration;

        public ServiceController(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        [HttpGet("GetServiceList")]
        public IActionResult GetServiceList()
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                DataTable dtResult = new ServiceDataLayer(_iConfiguration).GetServiceList();
                List<ServiceModel> svcList = new List<ServiceModel>();

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    ServiceModel svcDetails = new ServiceModel();
                    svcDetails.svcId = Convert.ToInt32(dtResult.Rows[i]["iSVCID"]);
                    svcDetails.compId = Convert.ToInt32(dtResult.Rows[i]["iCOID"]);
                    svcDetails.svcCode = dtResult.Rows[i]["vaSVCCODE"].ToString();
                    svcDetails.svcName = dtResult.Rows[i]["vaSVCNAME"].ToString();
                    svcDetails.svcDesc = dtResult.Rows[i]["vaSVCDESC"].ToString();
                    svcDetails.isActive = (bool)dtResult.Rows[i]["siSTATUS"];
                    svcDetails.price = decimal.Round(Convert.ToDecimal(dtResult.Rows[i]["mnPRICE"]), 2, MidpointRounding.AwayFromZero);
                    svcDetails.unit = dtResult.Rows[i]["vaSHORTNAME"].ToString();
                    svcList.Add(svcDetails);
                }

                resp.setResponse(1, new { svcList }, false);
            }
            catch (Exception e)
            {
                resp.setResponse(-1, e.Message.ToString(), true);
                return Unauthorized(resp);
            }


            return Ok(resp);
        }

        [HttpGet("GetServiceById")]
        public IActionResult GetServiceById([FromQuery] int Id)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                DataSet dsResult = new ServiceDataLayer(_iConfiguration).GetServiceDetails(Id);
                ServiceModel svcDetails = new ServiceModel();
                svcDetails.svcId = Convert.ToInt32(dsResult.Tables[0].Rows[0]["iSVCID"]);
                svcDetails.compId = Convert.ToInt32(dsResult.Tables[0].Rows[0]["iCOID"]);
                svcDetails.svcCode = dsResult.Tables[0].Rows[0]["vaSVCCODE"].ToString();
                svcDetails.svcName = dsResult.Tables[0].Rows[0]["vaSVCNAME"].ToString();
                svcDetails.svcDesc = dsResult.Tables[0].Rows[0]["vaSVCDESC"].ToString();
                svcDetails.isActive = (bool)dsResult.Tables[0].Rows[0]["siSTATUS"];
                svcDetails.price = decimal.Round(Convert.ToDecimal(dsResult.Tables[0].Rows[0]["mnPRICE"]), 2, MidpointRounding.AwayFromZero);
                svcDetails.unit = dsResult.Tables[0].Rows[0]["vaSHORTNAME"].ToString();

                resp.setResponse(1, new { svcDetails }, false);
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
