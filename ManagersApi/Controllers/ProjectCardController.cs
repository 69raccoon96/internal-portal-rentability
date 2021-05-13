using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [ApiController]
    [Route("projectcard")]
    public class ProjectCardController : ControllerBase
    {
        [HttpGet]
        public List<Project> GetProject([FromQuery(Name = "id")] int id)
        {
            var db  = new DataBase();
            return db.GetProjectsById(id);
        }
    }
}