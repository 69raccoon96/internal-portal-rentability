using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManagersApi.JiraWorkers
{
    public class JiraProvider
    {
        private  MongoClient _client = new MongoClient(
            "mongodb+srv://69raccoon96:,fhf,fY1@cluster0.zdu2b.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        public async Task JiraApi()
        {
            var managSet = new HashSet<Manager>();

            var projSet = new HashSet<Project>();
            var moduleSet = new HashSet<Module>();
            var taskSet = new HashSet<IssueTask>();

            var dic = new Dictionary<string, int>();


            var jira = Jira.CreateRestClient("https://jira.66bit.ru/jira/", "69raccoon96", ",fhf,fY1");


            //Types = 10205:Ошибка,10408:Новая функциональность,10000:Epic,10407:Улучшение,10001:Story,10201:Подзадача,10204:История,10200:Задача

            //for more results need to rewrite with IssueSearchOptions and while-cycle
            var issuesandepics = jira.Issues.GetIssuesFromJqlAsync("", 600, 0).Result;

            var managid = 1;
            Console.WriteLine("try get issues and epics");
            foreach (var i in issuesandepics)
            {
                if (dic.ContainsKey(i.AssigneeUser.Key))
                    continue;
                dic[i.AssigneeUser.Key] = managid;
                if (i.AssigneeUser.DisplayName.Split(" ").Length == 1)
                    managSet.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[0], "",
                        i.AssigneeUser.AvatarUrls.Large));
                else if (i.AssigneeUser.DisplayName == "Kornilov Valentin" ||
                         i.AssigneeUser.DisplayName.Split(" ").Length == 3)
                    managSet.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[1],
                        i.AssigneeUser.DisplayName.Split(" ")[0], i.AssigneeUser.AvatarUrls.Large));
                else
                    managSet.Add(new Manager(managid++, i.AssigneeUser.DisplayName.Split(" ")[0],
                        i.AssigneeUser.DisplayName.Split(" ")[1], i.AssigneeUser.AvatarUrls.Large));
            }

            var issues = issuesandepics.Where(x => x.Type.Name != "Epic").ToList();
            var epics = issuesandepics.Where(x => x.Type.Name == "Epic").ToList();
            Console.WriteLine("I get epics and issues");
            Console.WriteLine("Try get projects");
            var projects = await jira.Projects.GetProjectsAsync();

            var increment = 1;

            foreach (var i in projects)
            {
                dic[i.Key] = increment;
                projSet.Add(new Project(increment++, i.Name, "", "", 0, dic[i.LeadUser.Key], "Active",
                    new List<int>()));
            }
            Console.WriteLine("I got projects");
            Console.WriteLine("Try work with epics");
            var epicCounter = 1;
            var issueCounter = 1;
            foreach (var i in epics)
            {
                var epic = new Module();
                Console.WriteLine(i.Project);
                epic = new Module(epicCounter++, new List<int>(), i.Key.Value, i.Summary);
                projSet.Where(x => x.Id == dic[i.Project]).First().ModulesId.Add(epic.Id);
                dic[i.Key.Value] = epic.Id;
                moduleSet.Add(epic);
            }
            Console.WriteLine("Try work with issues");
            foreach (var i in issues)
            {
                Console.WriteLine(i.Project);
                if (dic.ContainsKey(i["Epic Link"].ToString()))
                    moduleSet.First(x => x.Id == dic[i["Epic Link"].Value]).TasksId.Add(issueCounter);

                int time = 0;
                int total = 0;
                if (i.TimeTrackingData.OriginalEstimateInSeconds.HasValue)
                    time = (int) i.TimeTrackingData.OriginalEstimateInSeconds;
                if (i.TimeTrackingData.TimeSpentInSeconds.HasValue)
                    total = (int) i.TimeTrackingData.TimeSpentInSeconds;
                Console.WriteLine("added " +  i.Project);
                taskSet.Add(new IssueTask(issueCounter++, total, time, i.Status.Name == "Done", i.Key.Value));
            }

            Console.WriteLine("Try past to mongo");
            MongoDbAccess(managSet, projSet, taskSet, moduleSet);
        }

        private void MongoDbAccess(HashSet<Manager> managers, HashSet<Project> projects, HashSet<IssueTask> tasks,
            HashSet<Module> modules)
        {
           
            var dbList = _client.GetDatabase("Managers");

            foreach (var man in managers)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"FirstName", man.FirstName},
                    {"LastName", man.LastName},
                    {"Id", man.Id},
                    {"ImageUrl", man.ImageUrl}
                };
                dbList.GetCollection<BsonDocument>("Managers").InsertOne(document);
            }
            Console.WriteLine("Past managers");
            foreach (var proj in projects)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Title", proj.Title},
                    {"DateStart", new BsonDateTime(new DateTime())},
                    {"DateEnd", new BsonDateTime(new DateTime())},
                    {"CustomerId", proj.CustomerId},
                    {"ManagerId", proj.ManagerId},
                    {"ProjectStatus", proj.ProjectStatus},
                    {"ModuleIds", new BsonArray(proj.ModulesId)}
                };
                dbList.GetCollection<BsonDocument>("Projects").InsertOne(document);
            }
            Console.WriteLine("Past projects");

            foreach (var task in tasks)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Id", task.Id},
                    {"Total", task.Total},
                    {"TimePlanned", task.TimePlanned},
                    {"IsDone", task.IsDone},
                    {"Name", task.Name}
                };
                dbList.GetCollection<BsonDocument>("Tasks").InsertOne(document);
            }
            Console.WriteLine("Past tasks");

            foreach (var module in modules)
            {
                var document = new BsonDocument
                {
                    {"_id", new BsonObjectId(new ObjectId())},
                    {"Id", module.Id},
                    {"TasksId", new BsonArray(module.TasksId)},
                    {"Description", module.Description},
                    {"Name", module.Name}
                };
                dbList.GetCollection<BsonDocument>("Modules").InsertOne(document);
            }
            Console.WriteLine("Past modules");
        }
        private void JiraTest()
        {
            var jira = Jira.CreateRestClient("https://jira.66bit.ru/jira/", "69raccoon96", ",fhf,fY1");
            var c = from i in jira.Issues.Queryable
                where i.Key == "PZ-513"
                select i;
            var a = c.FirstOrDefault();
            a.GetIssueLinksAsync();
        }

        private void MongoTest()
        {
            var dbClient = new MongoClient("mongodb+srv://changingnickname:Kcu9a@cluster0.zdu2b.mongodb.net/test");
            var dbList = dbClient.GetDatabase("Managers");
            var dbCol = dbList.GetCollection<BsonDocument>("Managers").AsQueryable();

            foreach (var a in dbCol)
            {
                Console.WriteLine(a.ElementCount);
                Console.WriteLine(a);
            }
        }
    }
}