using System.Collections.Generic;

namespace ManagersApi.Entities
{
    public class AnaliticData
    {
        public int PlanedTime { get; set; }
        public int FactTime { get; set; }
        public List<AnalyticSubparagraph> Data { get; set; }

        public AnaliticData()
        {
            Data = new List<AnalyticSubparagraph>();
        }
    }
}