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
            var groups = Db.Groups
                .Include(g => g.CommanderGroups)
                .SelectMany(g => g.CommanderGroups)
                .Where(cg => cg.CommanderName == cmdrName)
                .Select(cg => cg.Group)
                .ToList();

            return groups;
        }

        public void AddCommanderToGroup(Group group, string cmdrName)
        {
            var newUserGroup = new CommanderGroup
            {
                Group = group,
                CommanderName = cmdrName
            };
            group.CommanderGroups.Add(newUserGroup);
            Db.CommanderGroups.Add(newUserGroup);
            Db.SaveChanges();
        }

        public IEnumerable<string> GetCommandersInGroup(Group group)
        {
            var cmdrs = Db.Groups
                .Include(g => g.CommanderGroups)
                .Where(g=>g == group)
                .SelectMany(g => g.CommanderGroups)
                .Select(cg => cg.CommanderName)
                .Distinct()
                .ToList();

            return cmdrs;
        }
    }
}
