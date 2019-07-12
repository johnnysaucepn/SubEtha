using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Howatworks.Matrix.Site
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
            //services.AddAutofac(ConfigureContainer);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MatrixDbContext>();
            services.AddDefaultIdentity<MatrixIdentityUser>()
                .AddEntityFrameworkStores<MatrixDbContext>();

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMatrix();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /*public ContainerBuilder ConfigureContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MatrixDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterModule(new EntityFrameworkModule());
            //builder.RegisterModule(new InMemoryModule());
            //builder.RegisterModule(new MongoModule());
            builder.RegisterModule(new ServiceModule(Configuration));
            //builderRegisterApiControllers(typeof(ServiceModule).Assembly);
            builder.RegisterBuildCallback(x =>
            {

                var groupRepo = x.Resolve<IGroupRepository>();
                // Seed data
                if (groupRepo.GetDefaultGroup() == null)
                {
                    groupRepo.Add(new Group(EntityFrameworkGroupRepository.DefaultGroupName));
                }
            });
            return builder;
        }*/

        /*public IContainer ConfigureContainer(IServiceCollection services)
        {
            var builder = ConfigureContainerBuilder();

            builder.Populate(services);
            return builder.Build();
        }*/
    }
}
