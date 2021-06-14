using System;

namespace ManagersApi.Entities
{
    public class ProjectCreation
    {
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        
    }
}