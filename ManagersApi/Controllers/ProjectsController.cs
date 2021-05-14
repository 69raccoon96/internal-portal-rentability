using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public List<CutedProject> GetProjects([FromQuery(Name = "partOfTheName")] string name)
        {
            var db = new DataBase();
            return db.GetProjects(name);
        }
    }
}