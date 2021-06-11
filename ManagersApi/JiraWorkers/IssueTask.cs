namespace ManagersApi.JiraWorkers
{
    public struct IssueTask
    {
        public IssueTask(int id, int total, int timePlanned, bool isDone, string name)
        {
            Id = id;
            TimePlanned = timePlanned;

            Total = total;
            IsDone = isDone;

            Name = name;
        }

        public int Id;
        public int Total;
        public int TimePlanned;
        public bool IsDone;
        public string Name;
    }
}