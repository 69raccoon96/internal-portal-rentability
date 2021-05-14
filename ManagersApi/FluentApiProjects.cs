using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagersApi
{
    public class FluentApiProjects
    {
        private IEnumerable<Project> currentData;

        public FluentApiProjects(List<Project> projects)
        {
            currentData = projects.AsEnumerable();
        }

        public FluentApiProjects SetProjectsBrackets(DateTime dateStart, DateTime dateEnd)
        {
            if(dateStart != default)
                currentData = currentData.Where(x => x.DateStart > dateStart);
            if(dateEnd != default)
                currentData = currentData.Where(x => x.DateEnd < dateEnd);
            return this;
        }

        public FluentApiProjects SetManagers(int[] managers)
        {
            if(managers.Length != 0)
                currentData = currentData.Where(x => managers.Contains(x.ManagerId));
            return this;
        }
        
        public FluentApiProjects SetCustomers(int[] customers)
        {
            if(customers.Length != 0)
                currentData = currentData.Where(x => customers.Contains(x.CustomerId));
            return this;
        }

        public FluentApiProjects SetProjectId(int id)
        {
            if(id != default)
                currentData = currentData.Where(x => x.Id == id);
            return this;
        }

        public FluentApiProjects SetProjectStatus(ProjectStatus status)
        {
            currentData = currentData.Where(x => x.ProjectStatus == status);
            return this;
        }

        public List<Project> GetProjects()
        {
            return currentData.ToList();
        }
    }
}