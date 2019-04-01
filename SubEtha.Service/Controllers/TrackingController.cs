using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SubEtha.Core.Entities;
using SubEtha.Core.Repositories;
using SubEtha.Domain;

namespace SubEtha.Service.Controllers
{
    public class TrackingController : Controller
    {
        private readonly ISessionEntityRepository _sessionRepoz;
        private readonly ILocationEntityRepository _locationRepoz;
        private readonly IStateEntityRepository<ShipStateEntity> _shipRepository;
        private readonly IGroupRepository _groupRepository;

        public TrackingController(ILocationEntityRepository locationRepoz, IStateEntityRepository<ShipStateEntity> shipRepository, IGroupRepository groupRepository, ISessionEntityRepository sessionRepoz)
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
            foreach (var user in group.Users)
            {
                var location = _locationRepoz.GetMostRecent(user, gameVersion);
                if (location == null) continue;
                var cmdr = _sessionRepoz.GetAtDateTime(user, gameVersion, location.TimeStamp);
                var ship = _shipRepository.GetAtDateTime(user, gameVersion, location.TimeStamp);
                results.Add(ToTrackingRepresentation(cmdr, location, ship));
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