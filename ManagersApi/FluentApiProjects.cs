using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagersApi
{
    /// <summary>
    /// Helps to filter data of project
    /// </summary>
    public class FluentApiProjects
    {
        private IEnumerable<Project> currentData;
        private MongoDB db;
        
        public FluentApiProjects(List<Project> projects)
        {
            currentData = projects.AsEnumerable();
            db = new MongoDB();
        }
        /// <summary>
        /// Filter projects with time borders
        /// </summary>
        /// <param name="dateStart">Left border</param>
        /// <param name="dateEnd">Right border</param>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetProjectsBorders(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart != default)
                currentData = currentData.Where(x => x.DateStart > dateStart);
            if (dateEnd != default)
                currentData = currentData.Where(x => x.DateEnd < dateEnd);
            return this;
        }
        /// <summary>
        /// Filter projects with managers IDs
        /// </summary>
        /// <param name="managers">Managers IDs</param>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetManagers(int[] managers)
        {
            if (managers.Length != 0)
                currentData = currentData.Where(x => managers.Contains(x.ManagerId));
            return this;
        }
        /// <summary>
        /// Filter projects with customers IDs
        /// </summary>
        /// <param name="customers">Customers IDs</param>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetCustomers(int[] customers)
        {
            if (customers.Length != 0)
                currentData = currentData.Where(x => customers.Contains(x.CustomerId));
            return this;
        }
        /// <summary>
        /// Filter projects with projects IDs
        /// </summary>
        /// <param name="id">Projects IDs</param>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetProjectId(params int[] id)
        {
            if (id.Length != 0)
                currentData = currentData.Where(x => id.Contains(x.Id));
            return this;
        }
        /// <summary>
        /// Filter projects with project status
        /// </summary>
        /// <param name="status">Project status</param>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetProjectStatus(ProjectStatus status)
        {
            if (status != ProjectStatus.Undefined)
                currentData = currentData.Where(x => x.ProjectStatus == status);
            return this;
        }
        /// <summary>
        /// Add information about manager to project
        /// </summary>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetManagerData()
        {
            foreach (var element in currentData)
            {
                element.Manager = db.GetManagerById(element.ManagerId);
            }

            return this;
        }
        /// <summary>
        /// Finish filtering projects and transform enumerable to list
        /// </summary>
        /// <returns>Returns themself</returns>
        public List<Project> GetProjects()
        {
            return currentData.ToList();
        }
        /// <summary>
        /// Add information about all customer to project
        /// </summary>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetCustomerData()
        {
            foreach (var element in currentData)
            {
                element.Customer = db.GetCustomerById(element.CustomerId);
            }

            return this;
        }
        /// <summary>
        /// Add information about overdue time to project
        /// </summary>
        /// <returns>Returns themself</returns>
        public FluentApiProjects SetOverdueData()
        {
            foreach (var project in currentData)
            {
                project.Modules = db.GetProjectModules(project.ModuleIds);
                var (time, task) = Utilities.GetOverdueModules(project);
                project.OverdueTime = time;
                project.OverdueTasks = task;
            }

            return this;
        }
    }
}