using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Managers
{
    public class Project
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
        [JsonProperty("title")]
        public string Title { get; }
        [JsonProperty("overdueTime")]
        public int OverdueTime { get; }
        [JsonProperty("overdueTasks")]
        public int OverdueTasks { get; }
        [JsonProperty("dateStart")]
        public DateTime DateStart { get; }
        [JsonProperty("dateEnd")]
        public DateTime DateEnd { get; }

        public Project(string firstName, string lastName, string title, int id, string _id, int overdueTime,
            int overdueTasks, DateTime dateStart, DateTime dateEnd)
        {
            this._id = _id;
            OverdueTime = overdueTime;
            OverdueTasks = overdueTasks;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Title = title;
        }
    }
}