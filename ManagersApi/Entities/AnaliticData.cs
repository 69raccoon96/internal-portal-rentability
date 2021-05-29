using System.Collections.Generic;

namespace ManagersApi.Entities
{
    public class AnaliticData
    {
        public int TimePlaned { get; set; }
        public int TimeSpent { get; set; }
        public List<AnalyticSubparagraph> Data { get; set; }

        public AnaliticData()
        {
            Data = new List<AnalyticSubparagraph>();
        }
    }
}