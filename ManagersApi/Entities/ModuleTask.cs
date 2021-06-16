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
        public ModuleTask(int id, int total, int timePlanned, bool isDone, string name)
        {
            Id = id;
            TimePlaned = timePlanned;

            Total = total;
            IsDone = isDone;

            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ModuleTask task)
                return false;
            if (task.Id != Id)
                return false;
            if (task.IsDone != IsDone)
                return false;
            if (task.Name != Name)
                return false;
            if (task.Total != Total)
                return false;
            if (task.TimePlaned != TimePlaned)
                return false;
            return true;
        }
    }
    
}