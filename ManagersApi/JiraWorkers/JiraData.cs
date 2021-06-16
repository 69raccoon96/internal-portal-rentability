using System.Collections.Generic;

namespace ManagersApi.JiraWorkers
{
    public class JiraData
    {
        public HashSet<Manager> Managers;
        public HashSet<Project> Projects;
        public HashSet<Module> Modules;
        public HashSet<ModuleTask> Tasks;

        public JiraData()
        {
            Managers = new HashSet<Manager>();
            Projects = new HashSet<Project>();
            Modules = new HashSet<Module>();
            Tasks = new HashSet<ModuleTask>();
        }
    }
}