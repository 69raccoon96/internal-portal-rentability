using System.Collections.Generic;

namespace ManagersApi.JiraWorkers
{
    public struct Project
    {
        public Project(int id, string title, string dateStart, string dateEnd, int customerId, int managerId, string projectStatus, List<int> modulesId)
        {
            Id = id;
            Title = title;
            DateStart = dateStart;
            DateEnd = dateEnd;
            CustomerId = customerId;
            ManagerId = managerId;
            ProjectStatus = projectStatus;
            ModulesId = modulesId;
        }

        public int Id;
        public string Title;
        public string DateStart;
        public string DateEnd;
        public int CustomerId;
        public int ManagerId;
        public string ProjectStatus;
        public List<int> ModulesId;
    }
}