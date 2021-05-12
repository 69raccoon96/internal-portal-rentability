using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Managers
{
    public class DataBase
    {
        private MongoClient Client;
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
            return collection.Find(_ => true).ToListAsync().Result;
        }

        public List<Project> GetProjects(HttpRequest request)
        {
            var a = request.ReadFormAsync().Result;
            var name = a["partOfTheName"].ToString();
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(x => x.Title.ToUpper().Contains(name.ToUpper())).ToList();
        }

        public List<Project> GetProjectsById(HttpRequest request)
        {
            var a = request.ReadFormAsync().Result;
            var id = int.Parse(a["id"]);
            var db = Client.GetDatabase("Managers");
            var collection = db.GetCollection<Project>("Projects");
            return collection.Find(x => x.Id == id).ToList();
        }
    }
}