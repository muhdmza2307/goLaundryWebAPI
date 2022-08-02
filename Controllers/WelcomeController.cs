using goLaundryWebAPI.Data;
using goLaundryWebAPI.Models;
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
    public class WelcomeController : ControllerBase
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            ResponseModel resp = new ResponseModel();
            resp.setResponse(1, "Test Web API laundry", false);
            return Ok(resp);
        }
    }
}
