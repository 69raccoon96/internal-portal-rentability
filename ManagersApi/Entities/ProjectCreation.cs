using System;

namespace ManagersApi.Entities
{
    public class ProjectCreation
    {
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Manager { get; set; }
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        
    }
}