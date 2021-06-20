using System;
using System.Linq;
using System.Security.Claims;

namespace ManagersApi
{
    /// <summary>
    /// Class which represent methods that can not be storage at one class
    /// </summary>
    public static class 
        Utilities
    {
        /// <summary>
        /// Calculate sum of overdue time in all modules of project and count of overdue modules
        /// </summary>
        /// <param name="project">Project to inspect</param>
        /// <returns>Returns tuple, where first element is time which manager overdue at all modules of current project
        /// and second is count of modules which was overdue</returns>
        public static Tuple<double, int> GetOverdueModules(Project project)
        {
            var overdueTime = 0.0;
            var overdueTask = 0;
            foreach (var module in project.Modules)
            {
                var (time, task) = GetOverdueTask(module);
                overdueTask += task;
                overdueTime += time;
            }

            return new Tuple<double, int>(overdueTime, overdueTask);
        }

        /// <summary>
        /// Calculate sum of overdue time in all task of module and count of overdue tasks
        /// </summary>
        /// <param name="module">Module to inspect</param>
        /// <returns>Returns tuple, where first element is time which manager overdue at all tasks of current module
        /// and second is count of tasks which was overdue</returns>
        public static Tuple<double, int> GetOverdueTask(Module module)
        {
            var overdueTask = 0;
            var overdueTime = 0.0;
            foreach (var task in module.Tasks)
            {
                var dif = task.TimePlanned - task.Total;
                if (dif >= 0) continue;
                overdueTime += dif * -1;
                overdueTask++;
            }

            return new Tuple<double, int>(overdueTime, overdueTask);
        }

        /// <summary>
        /// Calculate planed and factual time for current module
        /// </summary>
        /// <param name="module">Module to inspect</param>
        /// <returns>Returns tuple where first element is time planed and second is factial time</returns>
        public static Tuple<double, double> GetTimePlanedAndFact(Module module)
        {
            var timePlaned = 0.0;
            var timeFact = 0.0;
            foreach (var task in module.Tasks)
            {
                timeFact += task.Total;
                timePlaned += task.TimePlanned;
            }

            return new Tuple<double, double>(timePlaned, timeFact);
        }

        /// <summary>
        /// Checking and parsing user ID and user type from token
        /// </summary>
        /// <param name="identity">Data from token</param>
        /// <returns>Returns tuple where first element is user ID and second is user type or return null if data is incorrect</returns>
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
            if (!Enum.TryParse<UserType>(stringRole, out var role))
                return null;
            if (!int.TryParse(sid, out var id))
                return null;
            return new Tuple<int, UserType>(id, role);
        }
    }
}
