using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("managers")]
    [EnableCors]
    public class ManagerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new DataBase();
            return Ok(db.GetManagers());
        }
    }
}