using System;

namespace ManagersApi
{
    public static class Utilities
    {
        public static Tuple<int, int> GetOverdueTasks(Project project)
        {
            var overdueTime = 0;
            var overdueTask = 0;
            foreach (var module in project.Modules)
            {
                foreach (var task in module.Tasks)
                {
                    var dif = task.TimePlaned - task.Total;
                    if (dif < 0)
                    {
                        overdueTime += dif * -1;
                        overdueTask++;
                    }
                }
            }
            return new Tuple<int, int>(overdueTime, overdueTask);
        }
    }
}