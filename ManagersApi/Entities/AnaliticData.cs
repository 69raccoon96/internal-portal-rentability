using System.Collections.Generic;

namespace ManagersApi.Entities
{
    public class AnaliticData
    {
        public double TimePlanned { get; set; }
        public double TimeSpent { get; set; }
        public List<AnalyticSubparagraph> Data { get; set; }

        public AnaliticData()
        {
            Data = new List<AnalyticSubparagraph>();
        }
    }
}
