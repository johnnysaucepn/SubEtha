using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkGroupRepository: EntityFrameworkRepository<Group>, IGroupRepository
    {
        public EntityFrameworkGroupRepository(DbContext db) : base(db)
        {
        }

        public IEnumerable<Group> GetRange(int skip, int take)
        {
            return Db.Set<Group>().Skip(skip).Take(take);
        }

        public Group GetByName(string name)
        {
            return Db.Set<Group>().FirstOrDefault(x => x.Name == name);
        }

        public Group GetDefaultGroup()
        {
            return GetByName(Group.DefaultGroupName);
        }

        public IEnumerable<Group> GetByUser(string userName)
        {
            return Db.Set<Group>().Where(x => x.Users.Select(y => y.UserName).Contains(userName));
        }

        public void AddUserToGroup(Group group, string userName)
        {
            var user = Db.Set<MatrixIdentityUser>().First(x => x.UserName == userName);
            group.Users.Add(user);
            Db.Update(group);
        }
    }
}
