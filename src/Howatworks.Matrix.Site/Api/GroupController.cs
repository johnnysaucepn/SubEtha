using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    [Authorize]
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
            return Ok(_groupRepoz.Query().ToList());
        }

        [HttpGet]
        [Route("{cmdrName}/Groups")]
        public IActionResult GetCommandersGroups(string cmdrName)
        {
            if (string.IsNullOrWhiteSpace(cmdrName))
            {
                return NotFound();
            }

            var existingGroups = _groupRepoz.GetByCommander(cmdrName).ToList();
            if (existingGroups.Any())
            {
                return Ok(existingGroups);
            }

            var defaultGroup = _groupRepoz.GetDefaultGroup();
            _groupRepoz.AddCommanderToGroup(defaultGroup, cmdrName);

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
        public IActionResult AddCommanderToGroup(string group, string cmdrName)
        {
            if (string.IsNullOrWhiteSpace(cmdrName))
            {
                return NotFound();
            }

            var existingGroup = _groupRepoz.GetByName(group);
            if (existingGroup == null)
            {
                return NotFound();
            }

            _groupRepoz.AddCommanderToGroup(existingGroup, cmdrName);
            return Ok();
        }
    }
}
