using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.InMemory
{
    public class InMemoryGroupRepository: InMemoryRepository<Group>, IGroupRepository
    {
        public const string DefaultGroupName = "Default";

        public InMemoryGroupRepository(InMemoryDbContext<Group> db) : base(db)
        {

        }

        public IList<Group> GetRange(int skip, int take)
        {
            return Db.AsQueryable().Skip(skip).Take(take).ToList();
        }

        public Group GetByName(string name)
        {
            return Db.AsQueryable().FirstOrDefault(x => x.Name == name);
        }

        public Group GetDefaultGroup()
        {
            return GetByName(DefaultGroupName);
        }

        public IList<Group> GetByCommander(string cmdrName)
        {
            return Db.AsQueryable()
                .Where(g => g.CommanderGroups.Select(ug => ug.CommanderName).Contains(cmdrName))
                .ToList();
        }

        public void AddCommanderToGroup(Group group, string cmdrName)
        {
            var newUserGroup = new CommanderGroup
            {
                GroupId = group.Id,
                Group = group,
                CommanderName = cmdrName
            };
            group.CommanderGroups.Add(newUserGroup);
            Db.CreateOrUpdate(group);
        }

    }
}
