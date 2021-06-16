using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        
        public Module(int id, List<int> tasksId, string description, string name)
        {
            Id = id;
            TasksId = tasksId.ToArray();
            Description = description;
            Name = name;
        }

        public Module()
        {
            
        }

        public void AddId(int id)//TODO временный костыль, чтобы поддержать работу JiraProvider
        {
            var current = TasksId.ToList();
            current.Add(id);
            TasksId = current.ToArray();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Module module)
                return false;
            if (module.Id != Id)
                return false;
            if (module.Name != Name)
                return false;
            if (module.Description != Description)
                return false;
            if (!module.TasksId.AsEnumerable().SequenceEqual(TasksId))
                return false;
            return true;
        }
    }
}