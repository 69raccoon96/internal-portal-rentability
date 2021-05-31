using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    public class CutProject
    {
        public CutProject(int projectId, string projectTitle)
        {
            Id = projectId;
            Title = projectTitle;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}