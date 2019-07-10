using System.Collections.Generic;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
{
    public interface IGroupRepository : IRepository<Group, long>
    {
        IEnumerable<Group> GetRange(int skip, int take);
        Group GetByName(string name);
        Group GetDefaultGroup();
        IEnumerable<Group> GetByUser(string user);
        void AddUserToGroup(Group group, string user);
    }
}
