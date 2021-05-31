using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class User
    {

        public int Id { get; set; }
        [JsonIgnore]
        public string Login { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public UserType UserType { get; set; }
        [BsonIgnore]
        public string Token { get; set; }
        [BsonIgnore]
        public string FirstName { get; set; }
        [BsonIgnore]
        public string LastName { get; set; }
    }
}