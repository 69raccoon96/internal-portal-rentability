using System;
using System.Collections.Generic;
using System.Linq;
using ManagersApi.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("managers")]
    [EnableCors]
    public class ManagerController : BaseControllerWithDb
    {
        public ManagerController(IDataBase db) : base(db)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.GetManagers());
        }
    }
}