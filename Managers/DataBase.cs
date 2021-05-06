using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Managers
{
    public class DataBase
    {
        public MongoClient Client;
        public DataBase()
        {
            Client =  new MongoClient("mongodb+srv://69raccoon96:,fhf,fY1@cluster0.zdu2b.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            /*var database = Client.GetDatabase("Managers");
            var collection = database.GetCollection<User>("users");
            var list = collection.Find(_ => true).ToList();*/
        }

        public User GetUser()
        {
            return null;
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

        public List<User> GetUsers()
        {
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<User>("users");
            var a = collection.Find(_ => true).ToListAsync().Result;
            return collection.Find(_ => true).ToListAsync().Result;
        }
    }
}