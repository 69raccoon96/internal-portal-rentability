using System.Collections.Generic;
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
        [HttpGet]
        public IActionResult GetProjects([FromQuery(Name = "partOfTheName")] string name)
        {
            name ??= "";
            return Ok(db.GetProjects(name));
        }
    }
}