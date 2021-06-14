using System;
using ManagersApi.DataBase;
using ManagersApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MongoDB.Bson;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("create")]
    [EnableCors]
    public class ProjectCreatorController : BaseControllerWithDb
    {
        public ProjectCreatorController(IDataBase db) : base(db)
        {
        }

        [HttpPost("projectcard")]
        public IActionResult CreateProject([FromBody] ProjectCreation projectCreation)
        {
            var customerId = db.GetCustomerIdByName(projectCreation.Customer);
            var project = db.GetProjectById(projectCreation.Id);
            project.CustomerId = customerId;
            project.DateStart = projectCreation.DateStart;
            project.DateEnd = projectCreation.DateEnd;
            if(db.UpdateProject(project))
                return Ok();
            return BadRequest();
        }
    }
}