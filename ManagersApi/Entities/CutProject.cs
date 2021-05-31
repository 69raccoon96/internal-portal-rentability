using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagersApi
{
    [BsonIgnoreExtraElements]
    public class CutProject
    {
        public CutProject(int projectId, string projectTitle, int managerId)
        {
            Id = projectId;
            Title = projectTitle;
            ManagerId = managerId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public int ManagerId { get; set; }
    }
}