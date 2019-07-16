using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    public class TrackingController : Controller
    {
        private readonly ISessionEntityRepository _sessionRepoz;
        private readonly ILocationEntityRepository _locationRepoz;
        private readonly IShipEntityRepository _shipRepository;
        private readonly IGroupRepository _groupRepository;

        public TrackingController(ILocationEntityRepository locationRepoz, IShipEntityRepository shipRepository, IGroupRepository groupRepository, ISessionEntityRepository sessionRepoz)
        {
            _locationRepoz = locationRepoz;
            _shipRepository = shipRepository;
            _groupRepository = groupRepository;
            _sessionRepoz = sessionRepoz;
        }

        [HttpGet]
        [Route("{groupName}/{gameVersion}/Tracking")]
        public IActionResult GetTracking(string groupName, string gameVersion)
        {
            var group = _groupRepository.Query().FirstOrDefault(x => x.Name == groupName);
            if (group == null) return NotFound();


            var results = new List<dynamic>();
            foreach (var cmdr in group.CommanderGroups.Select(x => x.CommanderName))
            {
                var location = _locationRepoz.GetMostRecent(cmdr, gameVersion);
                if (location == null) continue;
                var session = _sessionRepoz.GetAtDateTime(cmdr, gameVersion, location.TimeStamp);
                var ship = _shipRepository.GetAtDateTime(cmdr, gameVersion, location.TimeStamp);
                results.Add(ToTrackingRepresentation(session, location, ship));
            }

            return Ok(new {
                Group = groupName,
                GameVersion = gameVersion,
                Tracking = results
            });
        }

        private static dynamic ToTrackingRepresentation(ISessionState session, ILocationState location, IShipState ship)
        {
            return new
            {
                session?.CommanderName,
                session?.GameMode,
                session?.Group,
                location?.StarSystem,
                location?.Body,
                location?.Station,
                location?.SignalSource,
                location?.SurfaceLocation,
                Ship = ship?.Type,
                ship?.HullIntegrity,
                ship?.ShieldsUp
            };
        }
    }
}