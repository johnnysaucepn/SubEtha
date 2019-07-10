using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.MongoDb
{
    public class MongoDbGroupRepository: MongoDbRepository<Group>, IGroupRepository
    {
        public const string DefaultGroupName = "Default";

        public MongoDbGroupRepository(MongoDbContext<Group> db) : base(db)
        {
        }

        public IEnumerable<Group> GetRange(int skip, int take)
        {
            return Db.AsQueryable().Skip(skip).Take(take);
        }

        public Group GetByName(string name)
        {
            return Db.AsQueryable().FirstOrDefault(x => x.Name == name);
        }

        public Group GetDefaultGroup()
        {
            return GetByName(DefaultGroupName);
        }

        public IEnumerable<Group> GetByUser(string userName)
        {
            return Db.AsQueryable().Where(x => x.Users.Select(y => y.UserName).Contains(userName));
        }

        public void AddUserToGroup(Group group, string userName)
        {
            // TODO: this won't really work, we need to tie user objects, not strings
            var user = new MatrixIdentityUser(userName);
            group.Users.Add(user);
            Db.CreateOrUpdate(group);
        }
    }
}
