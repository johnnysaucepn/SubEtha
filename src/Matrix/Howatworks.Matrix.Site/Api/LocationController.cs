using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Extensions;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    [Authorize(Policy = "ApiPolicy")]
    public class LocationController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationController));

        private readonly ILocationEntityRepository _locationRepoz;

        public LocationController(ILocationEntityRepository locationRepoz)
        {
            _locationRepoz = locationRepoz;
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Location")]
        public IActionResult GetLocation(string user, string gameVersion)
        {
            var entity = _locationRepoz.GetMostRecent(user, gameVersion);
            if (entity == null) return NotFound();
            return Ok(ToLocationRepresentation(user, gameVersion, entity));
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Location/History")]
        public IActionResult GetLocationHistory(string user, string gameVersion)
        {
            var entities = _locationRepoz.GetRange(user, gameVersion, 0, int.MaxValue);
            return Ok(ToLocationRepresentation(user, gameVersion, entities));
        }

        private static dynamic ToLocationRepresentation(string user, string gameVersion, IEnumerable<LocationStateEntity> entities)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Locations = entities.Select(x => ToLocationRepresentation(user, gameVersion, x))
            };
        }

        private static dynamic ToLocationRepresentation(string user, string gameVersion, LocationStateEntity entity)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Location = ToLocationRepresentation(entity)
            };
        }

        private static ILocationState ToLocationRepresentation(LocationStateEntity entity)
        {
            if (entity is null) return null;
            return new LocationState
            {
                TimeStamp = entity.TimeStamp,
                Body = new Body(entity.Body_Name, entity.Body_Type, entity.Body_Docked ?? false),
                SignalSource = new SignalSource(new LocalisedString(entity.SignalSource_Type_Symbol, entity.SignalSource_Type_Text),entity.SignalSource_Threat),
                StarSystem = new StarSystem(entity.StarSystem_Name, new[] {entity.StarSystem_Coords_X, entity.StarSystem_Coords_Y, entity.StarSystem_Coords_Z}),
                Station = new Station(entity.Station_Name, entity.Station_Type),
                SurfaceLocation = new SurfaceLocation(entity.SurfaceLocation_Landed ?? false, entity.SurfaceLocation_Latitude, entity.SurfaceLocation_Longitude)
            };
        }

        private static dynamic ToSystemRepresentation(string user, string gameVersion, IEnumerable<LocationStateEntity> entities)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Systems = entities?.RemoveSequentialRepeats(new StarSystemEntityComparer()).Select(x => ToSystemRepresentation(x))
            };
        }

        private static dynamic ToSystemRepresentation(LocationStateEntity entity)
        {
            return new
            {
                entity?.TimeStamp,
                Name = entity?.StarSystem_Name,
                Coords = new[] {entity?.StarSystem_Coords_X, entity?.StarSystem_Coords_Y, entity?.StarSystem_Coords_Z}
            };
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Systems/History")]
        public IActionResult GetSystemHistory(string user, string gameVersion)
        {
            var entities = _locationRepoz.GetRange(user, gameVersion, 0, int.MaxValue);
            return Ok(ToSystemRepresentation(user, gameVersion, entities));
        }

        [HttpPost]
        [Route("{cmdrName}/{gameVersion}/Location")]
        public IActionResult PostLocation(string cmdrName, string gameVersion, [FromBody]LocationState location)
        {
            var locationEntity = ToEntity(location, cmdrName, gameVersion);

            _locationRepoz.Add(locationEntity);

            Log.Info(JsonConvert.SerializeObject(locationEntity));

            return Ok();
        }

        private static LocationStateEntity ToEntity(ILocationState location, string cmdrName, string version)
        {
            return new LocationStateEntity
            {
                CommanderName = cmdrName,
                GameVersion = version,
                TimeStamp = location.TimeStamp,

                StarSystem_Name = location.StarSystem?.Name,
                StarSystem_Coords_X = location.StarSystem?.Coords[0] ?? 0,
                StarSystem_Coords_Y = location.StarSystem?.Coords[1] ?? 0,
                StarSystem_Coords_Z = location.StarSystem?.Coords[2] ?? 0,

                Body_Name = location.Body?.Name,
                Body_Type = location.Body?.Type,
                Body_Docked = location.Body?.Docked,

                SurfaceLocation_Landed = location.SurfaceLocation?.Landed,
                SurfaceLocation_Latitude = location.SurfaceLocation?.Latitude,
                SurfaceLocation_Longitude = location.SurfaceLocation?.Longitude,

                Station_Name = location.Station?.Name,
                Station_Type = location.Station?.Type,

                SignalSource_Threat = location.SignalSource?.Threat,
                SignalSource_Type_Symbol = location.SignalSource?.Type.Symbol,
                SignalSource_Type_Text = location.SignalSource?.Type.Text
            };
        }
    }
}