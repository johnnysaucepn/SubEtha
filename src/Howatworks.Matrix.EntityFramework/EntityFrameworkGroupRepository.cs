using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkGroupRepository: EntityFrameworkRepository<Group>, IGroupRepository
    {
        public EntityFrameworkGroupRepository(MatrixDbContext db) : base(db)
        {
        }

        public IList<Group> GetRange(int skip, int take)
        {
            return Db.Set<Group>().Skip(skip).Take(take).ToList();
        }

        public Group GetByName(string name)
        {
            return Db.Set<Group>().FirstOrDefault(x => x.Name == name);
        }

        public Group GetDefaultGroup()
        {
            return GetByName(Group.DefaultGroupName);
        }

        public IList<Group> GetByCommander(string cmdrName)
        {
            var groupIds = Db.CommanderGroups
                .Where(cg => cg.CommanderName == cmdrName)
                .Select(g => g.GroupId).ToList();

            var groups = Db.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToList();

            return groups.ToList();
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
            Db.CommanderGroups.Add(newUserGroup);
        }
    }
}
