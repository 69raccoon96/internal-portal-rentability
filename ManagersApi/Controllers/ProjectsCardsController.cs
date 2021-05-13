using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [ApiController]
    [Route("projectscards")]
    public class ProjectsCardsController : ControllerBase
    {
        [HttpGet]
        public List<Project> GetProject()
        {
            var db  = new DataBase();
            return db.GetProjectsCards();
        }
    }
}