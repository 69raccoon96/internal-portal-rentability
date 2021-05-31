using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ManagersApi.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("projects")]
    public class ProjectsController : BaseControllerWithDb
    {
        public ProjectsController(IDataBase db) : base(db)
        {
        }
        /// <summary>
        /// Validate user and get cutted projects
        /// </summary>
        /// <param name="name">Part of project name</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProjects([FromQuery(Name = "partOfTheName")] string name)
        {
            name ??= "";
            var projects = db.GetProjects(name);
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            if (role == UserType.Manager) 
                projects = projects.Where(x => x.ManagerId == id).ToList();
            return Ok(projects);
        }
    }
}