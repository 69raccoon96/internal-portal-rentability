using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [ApiController]
    [Route("projectcard")]
    public class ProjectCardController : ControllerBase
    {
        [HttpGet]
        public Project GetProject([FromQuery(Name = "id")] int id)
        {
            var db = new DataBase();
            var project = db.GetProjectsById(id);
            if (project == null)
                return null;
            project.Modules = db.GetProjectModules(project.ModuleIds);
            return project;
        }
    }
}