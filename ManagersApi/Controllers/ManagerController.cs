﻿using System;
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
        //[Authorize]
        [HttpGet]
        public IEnumerable<Manager> Get()
        {
            var db = new DataBase();
            return db.GetManagers();
        }
    }
}