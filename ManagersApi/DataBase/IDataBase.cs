using System.Collections.Generic;
using ManagersApi.Entities;

namespace ManagersApi.DataBase
{
    /// <summary>
    /// Interface which show which methods are needed to work with data base
    /// </summary>
    public interface IDataBase
    {
        /// <summary>
        /// Get all managers from data base
        /// </summary>
        /// <returns>List of managers</returns>
        public List<Manager> GetManagers();

        /// <summary>
        /// Get all customers from data base
        /// </summary>
        /// <returns>List of customers</returns>
        public List<Customer> GetCustomers();

        /// <summary>
        /// Get project by id from data base 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project</returns>
        public Project GetProjectById(int id);

        /// <summary>
        /// Get projects from database where name of project contains argument
        /// </summary>
        /// <param name="part">Part of project name</param>
        /// <returns>Project, which contains Name,ID and Manager ID</returns>
        public List<CutProject> GetProjects(string part);

        /// <summary>
        /// Get modules from database
        /// </summary>
        /// <param name="moduleIds">IDs of requested modules</param>
        /// <returns>List of modules</returns>
        public List<Module> GetProjectModules(int[] moduleIds);

        /// <summary>
        /// Get tasks from database
        /// </summary>
        /// <param name="taskIds">IDs of requested tasks</param>
        /// <returns>List of tasks</returns>
        public List<ModuleTask> GetModuleTasks(int[] taskIds);

        /// <summary>
        /// Get user from data base, which contain login and password from arguments
        /// </summary>
        /// <param name="login">login of user</param>
        /// <param name="password">password of user</param>
        /// <returns>User</returns>
        public User GetUser(string login, string password);

        /// <summary>
        /// Get manager with ID from arguments
        /// </summary>
        /// <param name="id">ID of requested manager</param>
        /// <returns>Manager</returns>
        public Manager GetManagerById(int id);

        /// <summary>
        /// Get customer with ID from arguments
        /// </summary>
        /// <param name="id">ID of requested customer</param>
        /// <returns>Customer</returns>
        public Customer GetCustomerById(int id);

        /// <summary>
        /// Get full data of all projects
        /// </summary>
        /// <returns>Projects</returns>
        public List<Project> GetAllProjectsData();
        /// <summary>
        /// Get not full data of projects
        /// </summary>
        /// <returns>Projects</returns>
        public List<Project> GetProjectsWithoutModules();

        public int GetManagerIdByName(string name);
        public int GetCustomerIdByName(string name);

        public bool UpdateProject(Project project);
    }
}