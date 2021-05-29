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
    public class AnalyticsController
    {
        [HttpGet("general")]
        public General GetGeneral([FromQuery(Name = "managersId")] int[] managerId,
            [FromQuery(Name = "projectId")] int projectId, [FromQuery(Name = "dateStart")] DateTime dateStart,
            [FromQuery(Name = "dateEnd")] DateTime dateEnd, [FromQuery(Name = "customerId")] int[] customerId,
            [FromQuery(Name = "type")] ProjectStatus status)
        {
            var db = new DataBase(); //TODO Перевести на DI контейнер и протащить бд через него
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
                ProjectsIds = projects.Select(x => x.Id).Distinct().ToList()
            };
            return response;
        }
        //TODO подумать как переписать, чтобы не нарушать DRY
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
                    var (overdueTime, overdueTask) = Utilities.GetOverdueModules(project);
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

        [HttpGet("overdue/modules")]
        public AnaliticData GetOverdueDataModules([FromQuery(Name = "projectsId")] int[] projectId)
        {
            var db = new DataBase();
            var projectsCards = db.GetAllProjects().Where(x => projectId.Contains(x.Id)).ToList();
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in projectsCards)
            {
                foreach (var module in project.Modules)
                {
                    var (timePlaned, timeFact) = Utilities.GetTimePlanedAndFact(module);
                    if (timeFact <= timePlaned) continue;
                    var elementToAdd = new AnalyticSubparagraph();
                    planedTime += timePlaned;
                    factTime += timeFact;
                    elementToAdd.Name = module.Name;
                    elementToAdd.TimePlaned = planedTime;
                    elementToAdd.TimeSpent = factTime;
                    elementToAdd.ProjectName = project.Title;
                    result.Data.Add(elementToAdd);
                }
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return result;
        }

        [HttpGet("overdue/tasks")]
        public AnaliticData GetOverdueDataTasks([FromQuery(Name = "projectsId")] int[] projectId)
        {
            var db = new DataBase();
            var projectsCards = db.GetAllProjects().Where(x => projectId.Contains(x.Id)).ToList();
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in projectsCards)
            {
                foreach (var module in project.Modules)
                {
                    foreach (var task in module.Tasks.Where(x => x.TimePlaned < x.Total))
                    {
                        
                        planedTime = task.TimePlaned;
                        factTime = task.Total;
                        result.Data.Add(new AnalyticSubparagraph
                            {TimePlaned = task.TimePlaned, TimeSpent = task.Total, Name = task.Name, ProjectName = project.Title});
                    }
                }
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return result;
        }
        [HttpGet("overrated/modules")]
        public AnaliticData GetOverratedDataModules([FromQuery(Name = "projectsId")] int[] projectId)
        {
            var db = new DataBase();
            var projectsCards = db.GetAllProjects().Where(x => projectId.Contains(x.Id)).ToList();
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in projectsCards)
            {
                foreach (var module in project.Modules)
                {
                    var (timePlaned, timeFact) = Utilities.GetTimePlanedAndFact(module);
                    if (timeFact >= timePlaned) continue;
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
        [HttpGet("overrated/tasks")]
        public AnaliticData GetOverratedDataTasks([FromQuery(Name = "projectsId")] int[] projectId)
        {
            var db = new DataBase();
            var projectsCards = db.GetAllProjects().Where(x => projectId.Contains(x.Id)).ToList();
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in projectsCards)
            {
                foreach (var module in project.Modules)
                {
                    foreach (var task in module.Tasks.Where(x => x.TimePlaned > x.Total))
                    {
                        planedTime = task.TimePlaned;
                        factTime = task.Total;
                        result.Data.Add(new AnalyticSubparagraph
                            {TimePlaned = task.TimePlaned, TimeSpent = task.Total, Name = task.Name, ProjectName = project.Title});
                    }
                }
            }

            result.TimeSpent = factTime;
            result.TimePlaned = planedTime;
            return result;
        }
        [HttpGet("projects")]
        public AnaliticData GetOverratedDataProjects([FromQuery(Name = "projectsId")] int[] projectId)
        {
            var db = new DataBase();
            var projectsCards = db.GetAllProjects().Where(x => projectId.Contains(x.Id)).ToList();
            var result = new AnaliticData();
            var planedTime = 0;
            var factTime = 0;
            foreach (var project in projectsCards)
            {
                var analyticModule = new AnalyticSubparagraph();
                foreach (var module in project.Modules)
                {
                    var (timePlaned, timeFact) = Utilities.GetTimePlanedAndFact(module);
                    if(timeFact <= timePlaned) continue;
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
            return result;
        }
    }
}