using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Howatworks.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SubEtha.Core.Entities;
using SubEtha.Core.Repositories;
using SubEtha.Service.Persistence;

namespace SubEtha.Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddAutofac(ConfigureContainer);
        }

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new InMemoryModule());
            //builder.RegisterModule(new MongoModule());
            builder.RegisterModule(new ServiceModule());
            //builderRegisterApiControllers(typeof(ServiceModule).Assembly);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IGroupRepository groupRepoz, IConfigLoader configLoader)
        {
            //var groupRepoz = container.Resolve<IGroupRepository>();
            if (groupRepoz.GetDefaultGroup() == null)
            {
                groupRepoz.Add(new Group(GroupRepository.DefaultGroupName));
            }

            var section = configLoader.GetConfigurationSection("SubEtha.Service");
            var url = section.Get<string>("ServiceBinding") ?? "http://+:8984/SubEtha/Service";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            app.UseMvc();
        }
    }
}
