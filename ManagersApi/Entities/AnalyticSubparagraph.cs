namespace ManagersApi.Entities
{
    public class AnalyticSubparagraph//TODO возможно стоит использовать структуру
    {
        private readonly int _timePlaned;
        private readonly int _timeSpent;
        private readonly string _projectName;
        public string Name { get; set; }
        public int TimePlaned { get; set; }
        public int TimeSpent { get; set; }
        
        public string ProjectName { get; set; }

        public AnalyticSubparagraph()
        {
            
        }
        public AnalyticSubparagraph(string name, int timePlaned, int timeSpent, string projectName)
        {
            _timePlaned = timePlaned;
            _timeSpent = timeSpent;
            _projectName = projectName;
            Name = name;
        }
    }
}