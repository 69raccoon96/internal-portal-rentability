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
    [Route("createProject")]
    [EnableCors]
    public class ProjectCreatorController : BaseControllerWithDb
    {
        public ProjectCreatorController(IDataBase db) : base(db)
        {
        }
        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectCreation projectCreation)
        {
            var projects = db.GetAllProjectsData();
            var currentId = projects.Max(x => x.Id);
            var project = new Project
            {
                Id = ++currentId,
                Title = projectCreation.Name,
                DateEnd = projectCreation.Date,
                DateStart = DateTime.Now,
                ManagerId = db.GetManagerIdByName(projectCreation.Manager),
                CustomerId = db.GetCustomerIdByName(projectCreation.Customer),
                ModuleIds = new int[0],
                _id = new ObjectId(),
                ProjectStatus = ProjectStatus.Active
            };
            db.PasteProjectToDataBase(project);
            return Ok();
        }
        
    }
}