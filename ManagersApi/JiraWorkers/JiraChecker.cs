using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ManagersApi.DataBase;

namespace ManagersApi.JiraWorkers
{
    public class JiraChecker
    {
        private const int TimeWait = 50000;
        private readonly JiraProvider jiraProvider;
        private readonly IDataBase db;
        public JiraChecker(IDataBase db)
        {
            jiraProvider = new JiraProvider();
            this.db = db;

        }
        public void RunJiraChecker()
        {
            while (true)
            {
                CheckAndUpdateJira();
                Thread.SpinWait(TimeWait);
            }
        }

        private void CheckAndUpdateJira()
        {
            var jiraData = jiraProvider.ParseAllJira().Result;
            db.UpdateManagers(GetManagersDifference(db.GetManagers(),jiraData.Managers));
            db.UpdateModules(GetModulesDifference(db.GetAllModules(), jiraData.Modules));
            db.UpdateProjects(GetProjectsDifference(db.GetProjectsWithoutModules(), jiraData.Projects));
            db.UpdateTasks(GetTasksDifference(db.GetAllModuleTask(), jiraData.Tasks));
        }

        private HashSet<Project> GetProjectsDifference(List<Project> mongo, IEnumerable<Project> jira)//TODO подумать как переделать на дженерики или интерфейсы
        {
            var difference = new HashSet<Project>();
            foreach (var jiraItem in jira)
            {
                foreach (var mongoItem in mongo)
                {
                    if(jiraItem.Id != mongoItem.Id)
                        continue;
                    if(!jiraItem.Equals(mongoItem))
                        difference.Add(jiraItem);
                }
            }

            return difference;
        }
        private HashSet<Module> GetModulesDifference(List<Module> mongo, IEnumerable<Module> jira)
        {
            var difference = new HashSet<Module>();
            foreach (var jiraItem in jira)
            {
                foreach (var mongoItem in mongo)
                {
                    if(jiraItem.Id != mongoItem.Id)
                        continue;
                    if(!jiraItem.Equals(mongoItem))
                        difference.Add(jiraItem);
                }
            }

            return difference;
        }
        private HashSet<ModuleTask> GetTasksDifference(List<ModuleTask> mongo, IEnumerable<ModuleTask> jira)
        {
            var difference = new HashSet<ModuleTask>();
            foreach (var jiraItem in jira)
            {
                foreach (var mongoItem in mongo)
                {
                    if(jiraItem.Id != mongoItem.Id)
                        continue;
                    if(!jiraItem.Equals(mongoItem))
                        difference.Add(jiraItem);
                }
            }

            return difference;
        }
        private HashSet<Manager> GetManagersDifference(List<Manager> mongo, IEnumerable<Manager> jira)
        {
            var difference = new HashSet<Manager>();
            foreach (var jiraItem in jira)
            {
                foreach (var mongoItem in mongo)
                {
                    if(jiraItem.Id != mongoItem.Id)
                        continue;
                    if(!jiraItem.Equals(mongoItem))
                        difference.Add(jiraItem);
                }
            }

            return difference;
        }
        
        
        public void InsertDataToDb(JiraData jiraData)//Run this method, when db is empty
        {
            Console.WriteLine("Try past to mongo");
            db.InsertManagers(jiraData.Managers);
            Console.WriteLine("Pasted managers");
            db.InsertProjects(jiraData.Projects);
            Console.WriteLine("Pasted projects");

            db.InsertTasks(jiraData.Tasks);
            Console.WriteLine("Pasted tasks");

            db.InsertModules(jiraData.Modules);
            Console.WriteLine("Pasted modules");
        }
    }
}