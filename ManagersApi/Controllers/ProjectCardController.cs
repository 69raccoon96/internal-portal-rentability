using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
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
            project.Customer = db.GetCustomerById(project.CustomerId);
            project.Manager = db.GetManagerById(project.ManagerId);
            return project;
        }
    }
}