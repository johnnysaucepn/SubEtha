using System.Collections.Generic;
using Howatworks.Matrix.Core.Entities;

namespace Howatworks.Matrix.Core.Repositories
{
    public interface IGroupRepository : IRepository<Group, long>
    {
        IList<Group> GetRange(int skip, int take);
        Group GetByName(string name);
        Group GetDefaultGroup();
        IList<Group> GetByCommander(string cmdrName);
        void AddCommanderToGroup(Group group, string cmdrName);
    }
}
