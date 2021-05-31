using System.Collections.Generic;

namespace ManagersApi.DataBase
{
    public interface IDataBase
    {
        public List<Manager> GetManagers();


        public List<Customer> GetCustomers();


        public Project GetProjectsById(int id);


        public List<CutProject> GetProjects(string part);


        public List<Module> GetProjectModules(int[] moduleIds);


        public List<ModuleTask> GetModuleTasks(int[] taskIds);


        public User GetUser(string login, string password);


        public Manager GetManagerById(int id);


        public Customer GetCustomerById(int id);


        public List<Project> GetAllProjectsData();

        public List<Project> GetProjectsWithoutModules();
    }
}