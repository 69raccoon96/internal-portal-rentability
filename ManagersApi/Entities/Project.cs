using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class Project 
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        [BsonIgnore]
        public Customer Customer { get; set; }
        [BsonIgnore]
        public Manager Manager { get; set; }

        public string Title { get; set; }
        [BsonIgnore]
        public int OverdueTime { get; set; }
        [BsonIgnore]
        public int OverdueTasks { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [JsonIgnore]

        public int CustomerId { get; set; }
        [JsonIgnore]

        public int ManagerId { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        [BsonIgnore] public List<Module> Modules { get; set; }
        [JsonIgnore][BsonElement] public int[] ModuleIds { get; set; }
        public Project(int id, string title, string dateStart, string dateEnd, int customerId, int managerId, string projectStatus, List<int> modulesId)
        {
            Id = id;
            Title = title;
            DateStart = DateTime.Parse(dateStart);
            DateEnd = DateTime.Parse(dateEnd);
            CustomerId = customerId;
            ManagerId = managerId;
            ProjectStatus = Enum.Parse<ProjectStatus>(projectStatus);
            ModuleIds = modulesId.ToArray();
        }
        public void AddId(int id)//TODO временный костыль, чтобы поддержать работу JiraProvider
        {
            var current = ModuleIds.ToList();
            current.Add(id);
            ModuleIds = current.ToArray();
        }


        public override bool Equals(object? obj)
        {
            if (obj is not Project project)
                return false;
            if (project.Id != Id)
                return false;
            if (project.Title != Title)
                return false;
            if (!project.ModuleIds.AsEnumerable().SequenceEqual(ModuleIds))
                return false;
            if (project.ProjectStatus != ProjectStatus)
                return false;
            return true;
        }
    }
}