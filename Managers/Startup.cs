using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Managers
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO раскидать эндпоинты по файлам
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin());
            var db = new DataBase();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapGet("/managers",
                    async context =>
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(db.GetManagers()));
                    });
                endpoints.MapGet("/customers",
                    async context =>
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(db.GetCustomers()));
                    });
                endpoints.MapGet("/users",
                    async context =>
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(db.GetUsers()));
                    });
                endpoints.MapGet("/projects",
                    async context => { await context.Response.WriteAsync(SomeMethod(context, db)); });
                endpoints.MapGet("/projectcard",
                    async context =>
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(db.GetProjectsById(context.Request)));
                    });
            });
        }

        private string SomeMethod(HttpContext context, DataBase db)
        {
            var data = db.GetProjects(context.Request).First();
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(Project), "FirstName", "LastName", "OverdueTime", "OverdueTime",
                "DateStart", "DateEnd", "OverdueTasks");
            var serializerSettings = new JsonSerializerSettings {ContractResolver = jsonResolver};
            return JsonConvert.SerializeObject(data, serializerSettings);
        }
    }
}