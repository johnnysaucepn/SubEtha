using System;
using System.Collections.Generic;
using System.Text;
using Howatworks.Matrix.Data.Entities;
using Howatworks.Matrix.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Howatworks.Matrix.EntityFramework
{
    public static class MatrixExtensions
    {
        public static IServiceCollection AddMatrix(this IServiceCollection services)
        {
            services.AddScoped<ILocationEntityRepository, EntityFrameworkLocationEntityRepository>();
            services.AddScoped<ISessionEntityRepository, EntityFrameworkSessionEntityRepository>();
            services.AddScoped<IShipEntityRepository, EntityFrameworkShipEntityRepository>();
            services.AddScoped<IGroupRepository, EntityFrameworkGroupRepository>();
            return services;
        }
    }
}
