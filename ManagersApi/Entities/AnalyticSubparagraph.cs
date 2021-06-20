namespace ManagersApi.Entities
{
    public class AnalyticSubparagraph//TODO возможно стоит использовать структуру
    {
        public string Name { get; set; }
        public double TimePlaned { get; set; }
        public double TimeSpent { get; set; }
        
        public string ProjectName { get; set; }

        public AnalyticSubparagraph()
        {
            
        }
        public AnalyticSubparagraph(string name, double timePlaned, double timeSpent, string projectName)
        {
            TimePlaned = timePlaned;
            TimeSpent = timeSpent;
            ProjectName = projectName;
            Name = name;
        }
    }
}
