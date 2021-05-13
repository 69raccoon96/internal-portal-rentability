using System.Collections.Generic;
using MongoDB.Driver;

namespace ManagersApi
{
    public class DataBase
    {
        private MongoClient Client;
        public DataBase()
        {
            Client =  new MongoClient("mongodb+srv://69raccoon96:,fhf,fY1@cluster0.zdu2b.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        }
        
        public List<Manager> GetManagers()
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Manager>("Managers");
            return collection.Find(_ => true).ToListAsync().Result;
        }
        public List<Customer> GetCustomers()
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Customer>("Customers");
            return collection.Find(_ => true).ToListAsync().Result;
        }

        public List<Project> GetProjectsById(int id)
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(x => x.Id == id).ToList();
        }
        public List<CutedProject> GetProjects(string part)
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<CutedProject>("Projects");
            return collection.Find(x => x.Title.ToUpper().Contains(part.ToUpper())).ToList();
        }
        public List<Project> GetProjectsCards()
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(_ => true).ToListAsync().Result;
        }
    }
}