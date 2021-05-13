using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Managers
{
    [BsonIgnoreExtraElements]
    public class Manager
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string _id { get; }
        [JsonProperty("id")]
        public int Id { get; }
        [JsonProperty("firstName")]
        public string FirstName { get; }
        [JsonProperty("lastName")]
        public string LastName { get; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; }
        

        public Manager(string firstName, string lastName, string imageUrl, int id, string _id)
        {
            this._id = _id;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ImageUrl = imageUrl;
        }

    }
}