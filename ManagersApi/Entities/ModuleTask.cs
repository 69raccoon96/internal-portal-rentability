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

        public double Total { get; set; }

        public double TimePlanned { get; set; }
        
        public bool IsDone { get; set; }
        public ModuleTask(int id, double total, double timePlanned, bool isDone, string name)
        {
            Id = id;
            TimePlanned = timePlanned;

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
            if (task.TimePlanned != TimePlanned)
                return false;
            return true;
        }
    }
    
}
