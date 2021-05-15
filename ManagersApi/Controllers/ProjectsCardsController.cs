using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("projectscards")]
    public class ProjectsCardsController : ControllerBase
    {
        [HttpGet]
        public List<Project> GetProject([FromQuery(Name = "managersId")] int[] managerId,
            [FromQuery(Name = "projectId")] int projectId, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customerId")] int[] customerId,
            [FromQuery(Name = "type")] ProjectStatus status)
        {
            var db = new DataBase();
            var projectsCards = db.GetProjectsCards();
            var projectsCleaner = new FluentApiProjects(projectsCards);
            var a = projectsCleaner
                .SetProjectStatus(status)
                .SetProjectId(projectId)
                .SetManagers(managerId)
                .SetCustomers(customerId)
                .SetProjectStatus(status)
                .SetProjectsBrackets(dateStart, dateEnd)
                .GetProjects();
            return a;
        }
    }
}