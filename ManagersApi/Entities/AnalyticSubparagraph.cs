namespace ManagersApi.Entities
{
    public class AnalyticSubparagraph//TODO возможно стоит использовать структуру
    {
        public string Name { get; set; }
        public int TimePlaned { get; set; }
        public int TimeSpent { get; set; }
        
        public string ProjectName { get; set; }

        public AnalyticSubparagraph()
        {
            
        }
        public AnalyticSubparagraph(string name, int timePlaned, int timeSpent, string projectName)
        {
            TimePlaned = timePlaned;
            TimeSpent = timeSpent;
            ProjectName = projectName;
            Name = name;
        }
    }
}