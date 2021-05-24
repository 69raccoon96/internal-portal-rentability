using System;
using System.Collections.Generic;
using System.Linq;
using ManagersApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("analytics")]
    public class Analytics
    {
        [HttpGet("general")]
        public General GetGeneral([FromQuery(Name = "managersId")] int[] managerId,
            [FromQuery(Name = "projectId")] int projectId, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customerId")] int[] customerId,
            [FromQuery(Name = "type")] ProjectStatus status)
        {
            var db = new DataBase();//TODO Перевести на DI контейнер и протащить бд через него
            var projectsCards = db.GetProjectsCards();
            var projectsCleaner = new FluentApiProjects(projectsCards);
            var projects = projectsCleaner
                .SetProjectStatus(status)
                .SetProjectId(projectId)
                .SetManagers(managerId)
                .SetCustomers(customerId)
                .SetProjectStatus(status)
                .SetProjectsBrackets(dateStart, dateEnd)
                .SetManagerData()
                .SetCustomerData()
                .SetOverdueData()
                .GetProjects();
            var response = new General
            {
                ManagersIds = projects.Select(x => x.ManagerId).Distinct().ToList(),
                ProjectIds = projects.Select(x => x.Id).Distinct().ToList()
            };
            return response;
        }

        [HttpGet("brief")]
        public List<Brief> GetBrief([FromQuery(Name = "managersId")] int[] managerId)
        {
            var result = new List<Brief>();
            var db = new DataBase();
            var managers = db.GetManagers().Where(x => managerId.Contains(x.Id)).ToList();
            var projects = db.GetAllProjects();
            foreach (var manager in managers)
            {
                var valueToAdd = new Brief();
                valueToAdd.FullName = manager.FirstName + " " + manager.LastName;
                var managerProjects = projects.Where(x => x.ManagerId == manager.Id);
                var allTime = 0;
                var projectsCount = 0;
                foreach (var project in managerProjects)
                {
                    var (overdueTime, overdueTask) = Utilities.GetOverdueTasks(project);
                    if (overdueTime == 0) continue;
                    projectsCount++;
                    allTime += overdueTime;
                }

                valueToAdd.OverdueTime = allTime;
                valueToAdd.ProjectsCount = projectsCount;
                result.Add(valueToAdd);

            }
            
            return result;
        }
    }
}