﻿using System.Collections.Generic;
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

        public IEnumerable<Group> GetByUser(string user)
        {
            return Db.AsQueryable().Where(x => x.Users.Contains(user));
        }

        public void AddUserToGroup(Group group, string user)
        {
            group.Users.Add(user);
            Db.CreateOrUpdate(group);
        }
    }
}
