using System.Collections.Generic;
using AutoMapper;
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
            var automapperConfig = new AutomapperConfigurationFactory().BuildConfiguration();
            services
                .Configure<ZealotConfiguration>(options =>
                {
                    Configuration.Bind(options);
                    options.ProjectsListPath = Configuration.GetValue<string>("projects-list-path");
                })
                .AddTransient<IRequestFileConverter, RequestFileConverter>()
                .AddTransient<IProjectService, ProjectService>()
                .AddTransient<IProjectRepository, ProjectRepository>()
                .AddTransient<IDirectoryInfoFactory, DirectoryInfoFactory>()
                .AddTransient<IFileInfoFactory, FileInfoFactory>()
                .AddTransient<IJsonFileConverter<Project>, JsonFileConverter<Project>>()
                .AddTransient<IJsonFileConverter<ProjectsConfigsList>, JsonFileConverter<ProjectsConfigsList>>()
                .AddTransient<IFile, FileWrap>()
                .AddTransient<IMapper, Mapper>(_ => new Mapper(automapperConfig))
                .AddCors()
                .AddMvc(opt =>
                {
                    opt.EnableEndpointRouting = false;
                    opt.Filters.Add(typeof(ApiValidationFilterAttribute));
                    opt.OutputFormatters.Insert(0, new ProjectOutputFormatter());
                })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
