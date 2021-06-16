using System;
using System.Text;
using ManagersApi.Authorization;
using ManagersApi.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ManagersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataBase, MongoDB>();
            services.AddCors(c => { c.AddPolicy("AllowOrigin", 
                options => 
                    options.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        
            ); });
            
            services.AddControllers();
            services.AddSwaggerGen(
                c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "ManagersApi", Version = "v1"}); });
            BsonClassMap.RegisterClassMap<CutProject>(cm =>
            {
                cm.MapProperty(project => project.Id);
                cm.MapProperty(project => project.Title);
                cm.MapProperty(project => project.ManagerId);
                cm.MapCreator(project => new CutProject(project.Id, project.Title, project.ManagerId));
            });
            var tokenKey = "This is my test private key";
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            services.AddSingleton(x =>
                new JWTAuthenticationManager(tokenKey, new MongoDB()));
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BsonSerializer.RegisterSerializer(new EnumSerializer<ProjectStatus>(BsonType.String));
            //var task = Task.Run(() => jiraProvider.JiraApi());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagersApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());   
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}