using System;

namespace ManagersApi.Entities
{
    public class ProjectCreation
    {
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Manager { get; set; }
        public DateTime Date { get; set; }
        
    }
}