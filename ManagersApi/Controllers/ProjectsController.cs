using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("projects")]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProjects([FromQuery(Name = "partOfTheName")] string name)
        {
            var db = new DataBase();
            name ??= "";
            return Ok(db.GetProjects(name));
        }
    }
}