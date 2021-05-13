using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string _id { get; }
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string ImageUrl { get; }
        
        public Customer(string firstName, string lastName, string imageUrl, int id, string _id)
        {
            this._id = _id;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
        }
    }
}