using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    public class Module
    {
        [JsonIgnoreAttribute]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [JsonIgnore] public int Id { get; set; }
        [JsonIgnore] public int[] TasksId { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        [BsonIgnore] public List<ModuleTask> Tasks { get; set; }
    }
}