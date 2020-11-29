using System.Collections.Generic;
using Howatworks.Matrix.Data.Entities;

namespace Howatworks.Matrix.Data.Repositories
{
    public interface IGroupRepository : IRepository<Group, long>
    {
        IList<Group> GetRange(int skip, int take);
        Group GetByName(string name);
        Group GetDefaultGroup();
        IList<Group> GetByCommander(string cmdrName);
        void AddCommanderToGroup(Group group, string cmdrName);
        IEnumerable<string> GetCommandersInGroup(Group group);
    }
}
