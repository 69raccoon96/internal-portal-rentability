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
        public IActionResult GetProject([FromQuery(Name = "managersIds")] int[] managerId,
            [FromQuery(Name = "projectId")] int projectId, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customersIds")] int[] customerId,
            [FromQuery(Name = "type")] ProjectStatus status)
        {
            var db = new DataBase();
            var projectsCards = db.GetProjectsWithoutModules();
            var projectsCleaner = new FluentApiProjects(projectsCards);
            projectsCleaner.SetProjectStatus(status)
                .SetProjectId(projectId)
                .SetManagers(managerId)
                .SetCustomers(customerId)
                .SetProjectStatus(status)
                .SetProjectsBrackets(dateStart, dateEnd)
                .SetManagerData()
                .SetCustomerData()
                .SetOverdueData();
            return Ok(projectsCleaner.GetProjects());
        }
    }
}