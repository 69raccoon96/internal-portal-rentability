using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class ModuleTask
    {
        [JsonIgnore] public int Id { get; set; }
        
        public string Name { get; set; }

        public int Total { get; set; }

        public int TimePlaned { get; set; }
        
        public bool IsDone { get; set; }
    }
}