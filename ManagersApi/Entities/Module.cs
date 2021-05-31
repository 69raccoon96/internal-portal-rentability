using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class Module
    {
        [JsonIgnore] public int Id { get; set; }
        [JsonIgnore] public int[] TasksId { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        [BsonIgnore] public List<ModuleTask> Tasks { get; set; }
    }
}