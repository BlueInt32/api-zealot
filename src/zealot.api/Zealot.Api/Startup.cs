using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SystemWrap;
using Zealot.Api.ApiHelpers;
using Zealot.Api.Middlewares;
using Zealot.Domain;
using Zealot.Domain.Objects;
using Zealot.Repository;
using Zealot.Repository.IO;
using Zealot.Services;

namespace Zealot.Api
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
            services
                .Configure<ZealotConfiguration>(Configuration.GetSection("ZealotConfiguration"))
                .PostConfigure<ZealotConfiguration>(options =>
                {
                    options.ProjectsListPath = Configuration.GetValue<string>("projects-list-path");
                })
                .AddTransient<IAnnexFileConverter, AnnexFileConverter>()
                .AddTransient<IProjectService, ProjectService>()
                .AddTransient<IProjectRepository, ProjectRepository>()
                .AddTransient<IDirectoryInfoFactory, DirectoryInfoFactory>()
                .AddTransient<IFileInfoFactory, FileInfoFactory>()
                .AddTransient<IJsonFileConverter<Project>, JsonFileConverter<Project>>()
                .AddTransient<IJsonFileConverter<List<Project>>, JsonFileConverter<List<Project>>>()
                .AddTransient<IFile, FileWrap>()
                .AddCors()
                .AddMvc(opt =>
                {
                    opt.EnableEndpointRouting = false;
                    opt.Filters.Add(typeof(ApiValidationFilterAttribute));
                    // opt.OutputFormatters.Insert(0, new ProjectOutputFormatter());
                    // opt.InputFormatters.Insert(0, new ProjectInputFormatter());
                })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                    opt.SerializerSettings.Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter(typeof(CamelCaseNamingStrategy))
                    };

                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("mypolicy");
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseMvc();
        }
    }
}
