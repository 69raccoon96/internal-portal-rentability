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

        public Project(string firstName, string lastName, string title, int id, string _id, int overdueTime,
            int overdueTasks, DateTime dateStart, DateTime dateEnd, int customerId)
        {
            this._id = _id;
            OverdueTime = overdueTime;
            OverdueTasks = overdueTasks;
            DateStart = dateStart;
            DateEnd = dateEnd;
            CustomerId = customerId;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Title = title;
        } 
    }
}