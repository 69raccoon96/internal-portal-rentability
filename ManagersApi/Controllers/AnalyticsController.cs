using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ManagersApi.DataBase;
using ManagersApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : BaseControllerWithDb
    {
        public AnalyticsController(IDataBase db) : base(db)
        {
        }
        /// <summary>
        /// Validate user and filter projects with parameters
        /// </summary>
        /// <param name="managerId">Managers IDs</param>
        /// <param name="projectId">Projects IDs</param>
        /// <param name="dateStart">Left border</param>
        /// <param name="dateEnd">Right border</param>
        /// <param name="customerId">Customers IDs</param>
        /// <param name="status">Status of project</param>
        /// <returns></returns>
        [HttpGet("general")]
        public General GetGeneral([FromQuery(Name = "managersIds")] int[] managerId,
            [FromQuery(Name = "projectsIds")] int[] projectId, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customersIds")] int[] customerId,
            [FromQuery(Name = "type")] ProjectStatus status)
        { 
            var projectsCards = db.GetProjectsWithoutModules();
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            if (role == UserType.Manager)
                managerId = new[] {id};
            var projectsCleaner = new FluentApiProjects(projectsCards);
            var projects = projectsCleaner
                .SetProjectStatus(status)
                .SetProjectId(projectId)
                .SetManagers(managerId)
                .SetCustomers(customerId)
                .SetProjectStatus(status)
                .SetProjectsBorders(dateStart, dateEnd)
                .SetManagerData()
                .SetCustomerData()
                .SetOverdueData()
                .GetProjects();
            var response = new General
            {
                ManagersIds = projects.Select(x => x.ManagerId).Distinct().ToList(),
                ProjectsIds = projects.Select(x => x.Id).Distinct().ToList()
            };
            return response;
        }
        /// <summary>
        /// Calculate overdue time and count of overdue projects for every manager
        /// </summary>
        /// <param name="managerId">Requested managers</param>
        /// <returns></returns>
        [HttpGet("brief")]
        public IActionResult GetBrief([FromQuery(Name = "managersIds")] int[] managerId)
        {
            var result = new List<Brief>();
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            if (role == UserType.Manager)
                managerId = new[] {id};
            var managers = db.GetManagers().Where(x => managerId.Contains(x.Id)).ToList();
            var projects = db.GetAllProjectsData();
            foreach (var manager in managers)
            {
                var valueToAdd = new Brief {FullName = manager.FirstName + " " + manager.LastName};
                var managerProjects = projects.Where(x => x.ManagerId == manager.Id);
                var allTime = 0;
                var projectsCount = 0;
                foreach (var project in managerProjects)
                {
                    var (overdueTime, overdueTask) = Utilities.GetOverdueModules(project);
                    if (overdueTime == 0) continue;
                    projectsCount++;
                    allTime += overdueTime;
                }

                valueToAdd.OverdueTime = allTime;
                valueToAdd.ProjectsCount = projectsCount;
                result.Add(valueToAdd);
            }

            return Ok(result);
        }
        /// <summary>
        /// Count overdue for modules
        /// </summary>
        /// <param name="projectId"> requested projects</param>
        /// <returns></returns>
        [HttpGet("overdue/modules")]
        public IActionResult GetOverdueDataModules([FromQuery(Name = "projectsIds")] int[] projectId)
        {
            var filter = new Func<int, int, bool>((timePlaned, timeFact) => timeFact <= timePlaned);
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            return Ok(GetDataForModules(projectId,filter, id, role));
        }
        /// <summary>
        /// Count overdue for tasks
        /// </summary>
        /// <param name="projectId"> requested projects</param>
        /// <returns></returns>
        [HttpGet("overdue/tasks")]
        public IActionResult GetOverdueDataTasks([FromQuery(Name = "projectsIds")] int[] projectId)
        {
            var filter = new Func<ModuleTask, bool>(x => x.TimePlaned < x.Total);
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            return Ok(GetDataForTasks(projectId, filter, id, role));
        }
        /// <summary>
        /// Count overrated for modules
        /// </summary>
        /// <param name="projectId"> requested projects</param>
        /// <returns></returns>
        [HttpGet("overrated/modules")]
        public IActionResult GetOverratedDataModules([FromQuery(Name = "projectsIds")] int[] projectId)
        {
            var filter = new Func<int, int, bool>((timePlaned, timeFact) => timeFact >= timePlaned);
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            return Ok(GetDataForModules(projectId,filter, id, role));
        }
        /// <summary>
        /// Count overrated for tasks
        /// </summary>
        /// <param name="projectId"> requested projects</param>
        /// <returns></returns>
        [HttpGet("overrated/tasks")]
        public IActionResult GetOverratedDataTasks([FromQuery(Name = "projectsIds")] int[] projectId)
        {
            var filter = new Func<ModuleTask, bool>(x => x.TimePlaned > x.Total);
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var (id, role) = Utilities.ParseClaims(ident);
            if (ident == null)
                BadRequest();
            return Ok(GetDataForTasks(projectId, filter, id, role));
        }
        /// <summary>
        /// Count overrated for project
        /// </summary>
        /// <param name="projectId">Requested project</param>
        /// <returns></returns>
        [HttpGet("projects")]
        public IActionResult GetOverratedDataProjects([FromQuery(Name = "projectsIds")] int[] projectId)
        {
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in GetClearedProjects(projectId))
            {
                var analyticModule = new AnalyticSubparagraph();
                foreach (var module in project.Modules)
                {
                    var (timePlaned, timeFact) = Utilities.GetTimePlanedAndFact(module);
                    if (timeFact <= timePlaned) continue;
                    analyticModule.TimePlaned += timePlaned;
                    analyticModule.TimeSpent += timeFact;
                }

                analyticModule.Name = project.Title;
                planedTime += analyticModule.TimePlaned;
                factTime += analyticModule.TimeSpent;
                result.Data.Add(analyticModule);
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return Ok(result);
        }

        private List<Project> GetClearedProjects(int[] projectsIds)
        {
            var projects = db.GetAllProjectsData();
            var projectCleaner = new FluentApiProjects(projects);
            return projectCleaner.SetProjectId(projectsIds).GetProjects();
        }

        private AnaliticData GetDataForTasks(int[] projectId, Func<ModuleTask,bool> filter, int id, UserType role)
        {
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            var projects = GetClearedProjects(projectId);
            if (role == UserType.Manager)
                projects = projects.Where(x => x.ManagerId == id).ToList();
            foreach (var project in projects)
            {
                foreach (var module in project.Modules)
                {
                    foreach (var task in module.Tasks.Where(filter))
                    {
                        planedTime += task.TimePlaned;
                        factTime += task.Total;
                        result.Data.Add(new AnalyticSubparagraph(task.Name, task.TimePlaned, task.Total,
                            project.Title));
                    }
                }
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return result;
        }

        private AnaliticData GetDataForModules(int[] projectId, Func<int, int, bool> filter, int id, UserType role)
        {
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            var projects = GetClearedProjects(projectId);
            if (role == UserType.Manager)
                projects = projects.Where(x => x.ManagerId == id).ToList();
            foreach (var project in projects)
            {
                foreach (var module in project.Modules)
                {
                    var (timePlaned, timeFact) = Utilities.GetTimePlanedAndFact(module);
                    if (filter(timePlaned, timeFact)) continue;
                    var elementToAdd = new AnalyticSubparagraph();
                    planedTime += timePlaned;
                    factTime += timeFact;
                    elementToAdd.ProjectName = project.Title;
                    elementToAdd.Name = module.Name;
                    elementToAdd.TimePlaned = planedTime;
                    elementToAdd.TimeSpent = factTime;
                    result.Data.Add(elementToAdd);
                }
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return result;
        }

        
    }
}