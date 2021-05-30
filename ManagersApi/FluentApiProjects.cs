using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagersApi
{
    public class FluentApiProjects
    {
        private IEnumerable<Project> currentData;
        private DataBase db;

        public FluentApiProjects(List<Project> projects)
        {
            currentData = projects.AsEnumerable();
            db = new DataBase();
        }

        public FluentApiProjects SetProjectsBrackets(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart != default)
                currentData = currentData.Where(x => x.DateStart > dateStart);
            if (dateEnd != default)
                currentData = currentData.Where(x => x.DateEnd < dateEnd);
            return this;
        }

        public FluentApiProjects SetManagers(int[] managers)
        {
            if (managers.Length != 0)
                currentData = currentData.Where(x => managers.Contains(x.ManagerId));
            return this;
        }

        public FluentApiProjects SetCustomers(int[] customers)
        {
            if (customers.Length != 0)
                currentData = currentData.Where(x => customers.Contains(x.CustomerId));
            return this;
        }

        public FluentApiProjects SetProjectId(params int[] id)
        {
            if (id.Length != 0)
                currentData = currentData.Where(x => id.Contains(x.Id));
            return this;
        }

        public FluentApiProjects SetProjectStatus(ProjectStatus status)
        {
            if(status != ProjectStatus.Undefined)
                currentData = currentData.Where(x => x.ProjectStatus == status);
            return this;
        }

        public FluentApiProjects SetManagerData()
        {
            foreach (var element in currentData)
            {
                element.Manager = db.GetManagerById(element.ManagerId);
            }

            return this;
        }

        public List<Project> GetProjects()
        {
            return currentData.ToList();
        }

        public FluentApiProjects SetCustomerData()
        {
            foreach (var element in currentData)
            {
                element.Customer = db.GetCustomerById(element.CustomerId);
            }

            return this;
        }

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