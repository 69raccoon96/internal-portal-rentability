using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string _id { get; }
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public string Title { get; }
        public int OverdueTime { get; }
        public int OverdueTasks { get; }
        public DateTime DateStart { get; }
        public DateTime DateEnd { get; }
        
        public int CustomerId { get; }
        
        public int ManagerId { get; }
        
        public ProjectStatus ProjectStatus { get; }

        public Project(string FirstName, string LastName, string Title, int Id, string _id, int OverdueTime,
            int OverdueTasks, DateTime DateStart, DateTime DateEnd, int CustomerId, int ManagerId, ProjectStatus ProjectStatus)
        {
            this._id = _id;
            this.OverdueTime = OverdueTime;
            this.OverdueTasks = OverdueTasks;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
            this.CustomerId = CustomerId;
            this.ManagerId = ManagerId;
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Title = Title;
            this.ProjectStatus = ProjectStatus;
        } 
    }
}