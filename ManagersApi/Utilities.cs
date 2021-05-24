using System;

namespace ManagersApi
{
    public static class Utilities
    {
        public static Tuple<int, int> GetOverdueModules(Project project)
        {
            var overdueTime = 0;
            var overdueTask = 0;
            foreach (var module in project.Modules)
            {
                var (time,task) = GetOverdueTask(module);
                overdueTask += task;
                overdueTime += time;
            }
            return new Tuple<int, int>(overdueTime, overdueTask);
        }

        public static Tuple<int, int> GetOverdueTask(Module module)
        {
            var overdueTask = 0;
            var overdueTime = 0;
            foreach (var task in module.Tasks)
            {
                var dif = task.TimePlaned - task.Total;
                if (dif >= 0) continue;
                overdueTime += dif * -1;
                overdueTask++;
            }
            return new Tuple<int, int>(overdueTime, overdueTask);
        }

        public static Tuple<int,int> GetTimePlanedAndFact(Module module)
        {
            var timePlaned = 0;
            var timeFact = 0;
            foreach (var task in module.Tasks)
            {
                timeFact += task.Total;
                timePlaned += task.TimePlaned;
            }
            return new Tuple<int, int>(timePlaned, timeFact);
        }
    }
}