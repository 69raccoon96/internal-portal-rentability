using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class Manager
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }

        public Manager(int id, string firstName, string lastName, string imageUrl)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Manager manager)
                return false;
            if (manager.Id != Id)
                return false;
            if (manager.FirstName != FirstName)
                return false;
            if (manager.LastName != LastName)
                return false;
            if (manager.ImageUrl != ImageUrl)
                return false;
            return true;
        }
    }
}