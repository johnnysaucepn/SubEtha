using System.Collections.Generic;
using SubEtha.Core.Entities;

namespace SubEtha.Core.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<Group> GetRange(int skip, int take);
        Group GetByName(string name);
        Group GetDefaultGroup();
        IEnumerable<Group> GetByUser(string user);
        void AddUserToGroup(Group group, string user);
    }
}
