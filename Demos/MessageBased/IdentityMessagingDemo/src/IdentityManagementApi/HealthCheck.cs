using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagementApi
{
    [Route("api/v1/healthcheck")]
    public class HealthCheckController : Controller
    {

        [HttpGet]
        [Route("")]
        public IActionResult CheckHealth()
        {
            return Ok("ok");
        }
    }
}
