using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers
{
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        /*
        Method: GET
        URI: http://localhost:10500/api/status
        Body: no body
        */
        [HttpGet]
        public string GetStatus()
        {
            return "Status OK";
        }
    }
}
