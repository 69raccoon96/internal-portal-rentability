using System.Collections.Generic;

namespace ManagersApi.Entities
{
    public class OverdueData
    {
        public int PlanedTime { get; set; }
        public int FactTime { get; set; }
        public List<AnalyticModule> Data { get; set; }

        public OverdueData()
        {
            Data = new List<AnalyticModule>();
        }
    }
}