using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using ManagersApi.DataBase;

namespace ManagersApi.JiraWorkers
{
    public class JiraProvider
    {
        public async Task<JiraData> ParseAllJira()
        {
            var jiraData = new JiraData();

            var dic = new Dictionary<string, int>();

            var jira = Jira.CreateRestClient("https://jira.66bit.ru/jira/", "vasyasin.dmitrii", "VtPfKmZyC58963214"); //dmitrii can't see PZ project
            //var jira = Jira.CreateRestClient("https://jira.66bit.ru/jira/", "69raccoon96", ",fhf,fY1"); //raccoon can't see TESTR project

            //need to rewrite using IssueSearchOptions for better results
            var issueAndEpics = jira.Issues.GetIssuesFromJqlAsync("", 600, 0).Result;

            var managid = 1;
            Console.WriteLine("try get issues and epics(and fill array with managers)");
            foreach (var i in issueAndEpics)
            {
                if (dic.ContainsKey(i.AssigneeUser.Key))
                    continue;
                dic[i.AssigneeUser.Key] = managid;
                if (i.AssigneeUser.DisplayName.Split(" ").Length == 1)
                    jiraData.Managers.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[0], "",
                        i.AssigneeUser.AvatarUrls.Large));
                else if (i.AssigneeUser.DisplayName == "Kornilov Valentin" ||
                         i.AssigneeUser.DisplayName.Split(" ").Length == 3)
                    jiraData.Managers.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[1],
                        i.AssigneeUser.DisplayName.Split(" ")[0], i.AssigneeUser.AvatarUrls.Large));
                else
                    jiraData.Managers.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[0],
                        i.AssigneeUser.DisplayName.Split(" ")[1], i.AssigneeUser.AvatarUrls.Large));
            }

            var issues = issueAndEpics.Where(x => x.Type.Name != "Epic").ToList();
            var epics = issueAndEpics.Where(x => x.Type.Name == "Epic").ToList();
            Console.WriteLine("I get epics and issues");
            Console.WriteLine("Try get projects");
            var projectsJira = await jira.Projects.GetProjectsAsync();

            var increment = 1;

            foreach (var i in projectsJira)
            {
                dic[i.Key] = increment;
                jiraData.Projects.Add(new Project(increment++, i.Name, "", "", -1, dic[i.LeadUser.Key], "Active",
                    new List<int>()));
            }

            Console.WriteLine("I got projects");
            Console.WriteLine("Try work with epics");
            var epicCounter = 1;
            var issueCounter = 1;
            foreach (var i in epics)
            {
                var epic = new Module(epicCounter++, new List<int>(), i.Key.Value, i.Summary);
                Console.WriteLine(i.Project);
                jiraData.Projects.First(x => x.Id == dic[i.Project]).AddId(epic.Id);
                dic[i.Key.Value] = epic.Id;
                jiraData.Modules.Add(epic);
            }

            Console.WriteLine("Try work with issues");
            foreach (var i in issues)
            {
                Console.WriteLine(i.Project);
                if (dic.ContainsKey(i["Epic Link"].ToString()))
                    jiraData.Modules.First(x => x.Id == dic[i["Epic Link"].Value]).AddId(issueCounter);

                var time = 0;
                var total = 0;
                if (i.TimeTrackingData.OriginalEstimateInSeconds.HasValue)
                    time = (double)(i.TimeTrackingData.OriginalEstimateInSeconds / 60.0 / 60.0);
                if (i.TimeTrackingData.TimeSpentInSeconds.HasValue)
                    total = (double)(i.TimeTrackingData.TimeSpentInSeconds / 60.0 / 60.0);
                Console.WriteLine("added " + i.Project);
                jiraData.Tasks.Add(new ModuleTask(issueCounter++, Math.Round(total, 2), Math.Round(time, 2), i.Status.Name == "Done", i.Key.Value));
            }

            return jiraData;
        }
    }
}
