using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubEtha.Core.Entities;
using SubEtha.Core.Repositories;

namespace SubEtha.Service.Controllers
{
    public class GroupController : Controller
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static readonly ILog Log = LogManager.GetLogger(typeof(GroupController));

        private readonly IGroupRepository _groupRepoz;

        public GroupController(IGroupRepository groupRepoz)
        {
            _groupRepoz = groupRepoz;
        }

        [HttpGet]
        [Route("Groups")]
        public IActionResult GetAllGroups()
        {
            return Ok(_groupRepoz.Query().Where(x => true));
        }

        [HttpGet]
        [Route("{user}/Groups")]
        public IActionResult GetUsersGroups(string user)
        {
            var existingGroups = _groupRepoz.GetByUser(user).ToList();
            if (existingGroups.Any()) return Ok(existingGroups);

            var defaultGroup = _groupRepoz.GetDefaultGroup();
            _groupRepoz.AddUserToGroup(defaultGroup, user);

            return Ok(new List<Group> {defaultGroup});
        }

        [HttpGet]
        [Route("Groups/{group}")]
        public IActionResult GetGroup(string group)
        {
            return Ok(_groupRepoz.GetByName(group));
        }

        [HttpPost]
        [Route("Groups")]
        public IActionResult AddGroup(string group)
        {
            var existingGroup = _groupRepoz.GetByName(group);
            if (existingGroup != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            var newGroup = new Group(group);
            _groupRepoz.Add(newGroup);
            return Ok(newGroup);
        }

        [HttpDelete]
        [Route("Groups/{group}")]
        public IActionResult RemoveGroup(string group)
        {
            var existingGroup = _groupRepoz.GetByName(group);
            if (existingGroup == null)
            {
                return NotFound();
            }
            _groupRepoz.Remove(existingGroup);
            return Ok();
        }

        [HttpPost]
        [Route("Groups/{group}")]
        public IActionResult AddUserToGroup(string group)
        {
            var existingGroup = _groupRepoz.GetByName(group);
            if (existingGroup == null)
            {
                return NotFound();
            }
            _groupRepoz.Remove(existingGroup);
            return Ok();
        }
    }
}
