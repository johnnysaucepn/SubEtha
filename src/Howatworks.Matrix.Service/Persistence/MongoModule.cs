using Autofac;
using MongoDB.Driver;
using Howatworks.Matrix.Core.Mongo;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.Service.Persistence
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MongoDbContext<>)).As(typeof(IDbContext<>)).SingleInstance();
            builder.Register(c => new MongoClient("mongodb://localhost:27017")).As<MongoClient>().SingleInstance();
            builder.Register(c =>
            {
                var client = c.Resolve<MongoClient>();
                var db = client.GetDatabase("MatrixService");
                return db;
            }).As<IMongoDatabase>().SingleInstance();
        }

    }
}
