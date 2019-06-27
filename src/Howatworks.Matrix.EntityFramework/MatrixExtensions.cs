using System;
using System.Collections.Generic;
using System.Text;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Howatworks.Matrix.EntityFramework
{
    public static class MatrixExtensions
    {
        public static IServiceCollection AddMatrix(this IServiceCollection services)
        {
            services.AddSingleton<ILocationEntityRepository, EntityFrameworkLocationEntityRepository>();
            services.AddSingleton<ISessionEntityRepository, EntityFrameworkSessionEntityRepository>();
            services.AddSingleton<IShipEntityRepository, EntityFrameworkShipEntityRepository>();
            services.AddSingleton<IGroupRepository, EntityFrameworkGroupRepository>();
            return services;
        }
    }
}
