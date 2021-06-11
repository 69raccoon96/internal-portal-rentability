using System.Collections.Generic;

namespace ManagersApi.JiraWorkers
{
    public struct Module
    {
        public Module(int id, List<int> tasksId, string description, string name)
        {
            Id = id;
            TasksId = tasksId;
            Description = description;
            Name = name;
        }

        public int Id;
        public List<int> TasksId;
        public string Description;
        public string Name;
    }
}