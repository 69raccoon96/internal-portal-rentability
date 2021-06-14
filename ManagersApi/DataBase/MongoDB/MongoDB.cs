using System.Collections.Generic;
using System.Linq;
using ManagersApi.DataBase;
using MongoDB.Driver;

namespace ManagersApi
{
    public class MongoDB : IDataBase
    {
        private MongoClient Client;
        private IMongoDatabase db;

        public MongoDB()
        {
            Client = new MongoClient(//TODO вынести в cfg
                "mongodb+srv://69raccoon96:,fhf,fY1@cluster0.zdu2b.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            db = Client.GetDatabase("Managers");
        }
        /// <summary>
        /// aaa
        /// </summary>
        /// <returns></returns>
        public List<Manager> GetManagers()
        {
            var collection = db.GetCollection<Manager>("Managers");
            return collection.Find(_ => true).ToListAsync().Result;
        }

        public List<Customer> GetCustomers()
        {
            var collection = db.GetCollection<Customer>("Customers");
            return collection.Find(_ => true).ToListAsync().Result;
        }

        public Project GetProjectById(int id)
        {
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(x => x.Id == id).First();
        }

        public List<CutProject> GetProjects(string part)
        {
            var collection = db.GetCollection<CutProject>("Projects");
            return collection.Find(x => x.Title.ToUpper().Contains(part.ToUpper())).ToList();
        }

        

        public List<Module> GetProjectModules(int[] moduleIds)
        {
            var collection = db.GetCollection<Module>("Modules");
            var modules = collection.Find(x => moduleIds.Contains(x.Id)).ToList();
            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].Tasks = GetModuleTasks(modules[i].TasksId);
            }

            return modules;
        }

        public List<ModuleTask> GetModuleTasks(int[] taskIds)
        {
            var collection = db.GetCollection<ModuleTask>("Tasks");
            return collection.Find(x => taskIds.Contains(x.Id)).ToList();
        }

        public User GetUser(string login, string password)
        {
            var collection = db.GetCollection<User>("users");
            return collection.Find(x => x.Login == login && x.Password == password).FirstOrDefault();
        }

        public Manager GetManagerById(int id)
        {
            var collection = db.GetCollection<Manager>("Managers");
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public Customer GetCustomerById(int id)
        {
            var collection = db.GetCollection<Customer>("Customers");
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<Project> GetAllProjectsData()
        {
            var collection = db.GetCollection<Project>("Projects");
            var projects = collection.Find(_ => true).ToList();
            foreach (var project in projects)
            {
                project.Modules = GetProjectModules(project.ModuleIds);
            }
            return projects;
        }
        public List<Project> GetProjectsWithoutModules()
        {
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(_ => true).ToListAsync().Result;
        }

        public int GetManagerIdByName(string name)
        {
            var collection = db.GetCollection<Manager>("Managers");
            var manager = collection.Find(_ => true).ToListAsync().Result;
            return manager.First(x => x.FirstName + " " + x.LastName == name).Id;
        }

        public int GetCustomerIdByName(string name)
        {
            var collection = db.GetCollection<Customer>("Customers");
            var customer = collection.Find(_ => true).ToListAsync().Result;
            return customer.First(x => x.FirstName + " " + x.LastName == name).Id;
        }

        public bool UpdateProject(Project project)
        {
            var filter = db.GetCollection<Project>("Projects").ReplaceOne(x => x.Id == project.Id, project);
            return filter.IsAcknowledged;
        }
    }
}