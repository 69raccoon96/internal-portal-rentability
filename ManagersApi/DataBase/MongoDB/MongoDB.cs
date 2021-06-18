using System;
using System.Collections.Generic;
using System.Linq;
using ManagersApi.DataBase;
using ManagersApi.JiraWorkers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManagersApi
{
    public class MongoDB : IDataBase
    {
        private MongoClient Client;
        private IMongoDatabase db;

        public MongoDB()
        {
            Client = new MongoClient( //TODO вынести в cfg
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

        public void InsertProjects(IEnumerable<Project> projects)
        {
            var dataToPast = new List<BsonDocument>();
            foreach (var proj in projects)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Id", proj.Id},
                    {"Title", proj.Title},
                    {"DateStart", new BsonDateTime(new DateTime())},
                    {"DateEnd", new BsonDateTime(new DateTime())},
                    {"CustomerId", proj.CustomerId},
                    {"ManagerId", proj.ManagerId},
                    {"ProjectStatus", proj.ProjectStatus},
                    {"ModuleIds", new BsonArray(proj.ModuleIds)}
                };
                dataToPast.Add(document);
            }

            db.GetCollection<BsonDocument>("Projects").InsertMany(dataToPast);
        }

        public void InsertTasks(IEnumerable<ModuleTask> tasks)
        {
            var dataToPast = new List<BsonDocument>();
            foreach (var task in tasks)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Id", task.Id},
                    {"Total", task.Total},
                    {"TimePlaned", task.TimePlanned},
                    {"IsDone", task.IsDone},
                    {"Name", task.Name}
                };
                dataToPast.Add(document);
            }

            db.GetCollection<BsonDocument>("Tasks").InsertMany(dataToPast);
        }

        public void InsertModules(IEnumerable<Module> modules)
        {
            var dataToPast = new List<BsonDocument>();
            foreach (var module in modules)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Id", module.Id},
                    {"TasksId", new BsonArray(module.TasksId)},
                    {"Description", module.Description},
                    {"Name", module.Name}
                };
                dataToPast.Add(document);
            }
            db.GetCollection<BsonDocument>("Modules").InsertMany(dataToPast);
        }

        public void InsertManagers(IEnumerable<Manager> managers)
        {
            var dataToPast = new List<BsonDocument>();
            foreach (var man in managers)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"FirstName", man.FirstName},
                    {"LastName", man.LastName},
                    {"Id", man.Id},
                    {"ImageUrl", man.ImageUrl}
                };
                dataToPast.Add(document);
            }
            db.GetCollection<BsonDocument>("Managers").InsertMany(dataToPast);
        }

        public List<Module> GetAllModules()
        {
            return db.GetCollection<Module>("Modules").Find(_ => true).ToList();
        }

        public List<ModuleTask> GetAllModuleTask()
        {
            return db.GetCollection<ModuleTask>("Tasks").Find(_ => true).ToList();
        }

        public void UpdateManagers(IEnumerable<Manager> managers)
        {
            var projectCollection = db.GetCollection<Manager>("Managers");
            var ids = managers.Select(x => x.Id);
            projectCollection.DeleteMany(x => ids.Contains(x.Id));
            projectCollection.InsertMany(managers);
        }

        public void UpdateProjects(IEnumerable<Project> projects)
        {
            var projectCollection = db.GetCollection<Project>("Projects");
            var ids = projects.Select(x => x.Id);
            projectCollection.DeleteMany(x => ids.Contains(x.Id));
            projectCollection.InsertMany(projects);
        }

        public void UpdateModules(IEnumerable<Module> modules)
        {
            var projectCollection = db.GetCollection<Module>("Module");
            var ids = modules.Select(x => x.Id);
            projectCollection.DeleteMany(x => ids.Contains(x.Id));
            projectCollection.InsertMany(modules);
        }

        public void UpdateTasks(IEnumerable<ModuleTask> tasks)
        {
            var projectCollection = db.GetCollection<ModuleTask>("Tasks");
            var ids = tasks.Select(x => x.Id);
            projectCollection.DeleteMany(x => ids.Contains(x.Id));
            projectCollection.InsertMany(tasks);
        }
    }
}