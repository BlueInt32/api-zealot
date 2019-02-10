using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SystemWrap;
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
                .AddTransient<IProjectService, ProjectService>()
                .AddTransient<IProjectRepository, ProjectRepository>()
                .AddTransient<IDirectoryInfoFactory, DirectoryInfoFactory>()
                // .AddTransient<IFileInfoFactory, FileInfoFactory>()
                .AddTransient<IObjectJsonDump<Project>, ObjectJsonDump<Project>>()
                .AddTransient<IFile, FileWrap>()
                .AddCors()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMvc();
        }
    }
}
