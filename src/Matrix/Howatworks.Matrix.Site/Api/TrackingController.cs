using System.Collections.Generic;
using Howatworks.Matrix.Data.Entities;
using Howatworks.Matrix.Data.Repositories;
using Howatworks.Matrix.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    [Authorize(Policy = "ApiPolicy")]
    public class TrackingController : Controller
    {
        private readonly ISessionEntityRepository _sessionRepoz;
        private readonly ILocationEntityRepository _locationRepoz;
        private readonly IShipEntityRepository _shipRepoz;
        private readonly IGroupRepository _groupRepoz;

        public TrackingController(ILocationEntityRepository locationRepoz, IShipEntityRepository shipRepoz, IGroupRepository groupRepoz, ISessionEntityRepository sessionRepoz)
        {
            _locationRepoz = locationRepoz;
            _shipRepoz = shipRepoz;
            _groupRepoz = groupRepoz;
            _sessionRepoz = sessionRepoz;
        }

        [HttpGet]
        [Route("{groupName}/{gameVersion}/Tracking")]
        public IActionResult GetTracking(string groupName, string gameVersion)
        {
            var group = _groupRepoz.GetByName(groupName);
            if (group == null) return NotFound();

            var results = new List<dynamic>();
            foreach (var cmdr in _groupRepoz.GetCommandersInGroup(group))
            {
                var location = _locationRepoz.GetMostRecent(cmdr, gameVersion);
                if (location == null) continue;
                var session = _sessionRepoz.GetAtDateTime(cmdr, gameVersion, location.TimeStamp);
                var ship = _shipRepoz.GetAtDateTime(cmdr, gameVersion, location.TimeStamp);
                results.Add(ToTrackingRepresentation(session, location, ship));
            }

            return Ok(new {
                Group = groupName,
                GameVersion = gameVersion,
                Tracking = results
            });
        }

        private static dynamic ToTrackingRepresentation(SessionStateEntity session, LocationStateEntity location, ShipStateEntity ship)
        {
            return new
            {
                session?.CommanderName,
                session?.GameMode,
                session?.Group,
                StarSystem = new StarSystem(location?.StarSystem_Name, new decimal[] {location?.StarSystem_Coords_X ?? 0, location?.StarSystem_Coords_Y ?? 0, location?.StarSystem_Coords_Z ?? 0}),
                Body = new Body(location?.Body_Name, location?.Body_Type, location?.Body_Docked ?? false),
                Station = new Station(location?.Station_Name, location?.Station_Type),
                SignalSource = new SignalSource(new LocalisedString(location?.SignalSource_Type_Symbol, location?.SignalSource_Type_Text), location?.SignalSource_Threat),
                SurfaceLocation = new SurfaceLocation(location?.SurfaceLocation_Landed ?? false, location?.SurfaceLocation_Latitude, location?.SurfaceLocation_Latitude),
                Ship = ship?.Type,
                ship?.HullIntegrity,
                ship?.ShieldsUp
            };
        }
    }
}