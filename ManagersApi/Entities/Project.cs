﻿using System;
using System.Collections.Generic;
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
        public string _id { get; set; }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Title { get; set; }
        public int OverdueTime { get; set; }
        public int OverdueTasks { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int CustomerId { get; set; }

        public int ManagerId { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        [BsonIgnore] public List<Module> Modules { get; set; }
        [JsonIgnore] public int[] ModuleIds { get; set; }
    }
}