using System;
using System.Linq;
using System.Security.Claims;

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

        public static Tuple<int, UserType> ParseClaims(ClaimsIdentity identity)
        {
            if (identity == null)
                return null;
            var stringRole = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).SingleOrDefault();
            var sid = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value).SingleOrDefault();
            if (stringRole == null || sid == null)
                return null;
            if(!Enum.TryParse<UserType>(stringRole, out var role))
                return null;
            if (!int.TryParse(sid, out var id))
                return null;
            return new Tuple<int, UserType>(id,role);
        }
    }
}