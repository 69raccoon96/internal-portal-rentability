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
    [Route("projectscards")]
    public class ProjectsCardsController : BaseControllerWithDb
    {
        public ProjectsCardsController(IDataBase db) : base(db)
        {
        }
        /// <summary>
        /// Validate user and filter projects with parameters
        /// </summary>
        /// <param name="managersIds">Managers IDs</param>
        /// <param name="projectsIds">Projects IDs</param>
        /// <param name="dateStart">Left border</param>
        /// <param name="dateEnd">Right border</param>
        /// <param name="customersIds">Customers IDs</param>
        /// <param name="status">Status of project</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProject([FromQuery(Name = "managersIds")] int[] managersIds,
            [FromQuery(Name = "projectsIds")] int[] projectsIds, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customersIds")] int[] customersIds,
            [FromQuery(Name = "type")] ProjectStatus status)
        {
            var projectsCards = db.GetProjectsWithoutModules();
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            if (role == UserType.Manager)
            {
                managersIds = new[] {id};
            }
            var projectsCleaner = new FluentApiProjects(projectsCards);
            projectsCleaner.SetProjectStatus(status)
                .SetProjectId(projectsIds)
                .SetManagers(managersIds)
                .SetCustomers(customersIds)
                .SetProjectStatus(status)
                .SetProjectsBorders(dateStart, dateEnd)
                .SetManagerData()
                .SetCustomerData()
                .SetOverdueData();
            return Ok(projectsCleaner.GetProjects());
        }
    }
}