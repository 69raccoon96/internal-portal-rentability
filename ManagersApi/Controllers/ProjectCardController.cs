using System;
using System.Collections.Generic;
using System.Security.Claims;
using ManagersApi.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("projectcard")]
    public class ProjectCardController : BaseControllerWithDb
    {
        public ProjectCardController(IDataBase db) : base(db)
        {
        }
        [HttpGet]
        public IActionResult GetProject([FromQuery(Name = "id")] int id)
        {
            var project = db.GetProjectsById(id);
            if (project == null)
                return null;
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (mangerId, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            if (role == UserType.Manager && project.ManagerId != mangerId)
                return BadRequest();
            project.Modules = db.GetProjectModules(project.ModuleIds);
            project.Customer = db.GetCustomerById(project.CustomerId);
            project.Manager = db.GetManagerById(project.ManagerId);
            return Ok(project);
        }
    }
}