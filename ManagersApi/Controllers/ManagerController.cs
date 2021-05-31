using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            var managers = db.GetManagers();
            if (role == UserType.Leader)
                return Ok(managers);
            return Ok(managers.Where(x => x.Id == id));
        }
    }
}