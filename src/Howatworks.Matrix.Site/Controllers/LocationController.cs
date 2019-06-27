using System.Collections.Generic;
using System.Linq;
using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Extensions;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Howatworks.Matrix.Site.Controllers
{
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

        private static dynamic ToLocationRepresentation(string user, string gameVersion, IEnumerable<ILocationState> entities)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Locations = entities.Select(x => ToLocationRepresentation(x))
            };
        }

        private static dynamic ToLocationRepresentation(string user, string gameVersion, ILocationState entity)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Location = ToLocationRepresentation(entity)
            };
        }

        private static dynamic ToLocationRepresentation(ILocationState entity)
        {
            return new
            {
                entity?.TimeStamp,
                entity?.Body,
                entity?.SignalSource,
                entity?.StarSystem,
                entity?.Station,
                entity?.SurfaceLocation
            };
        }

        private static dynamic ToSystemRepresentation(string user, string gameVersion, IEnumerable<ILocationState> entities)
        {
            return new
            {
                GameContext = new GameContext(gameVersion, user),
                Systems = entities?.RemoveSequentialRepeats(new StarSystemComparer()).Select(x => ToSystemRepresentation(x))
            };
        }

        private static dynamic ToSystemRepresentation(ILocationState entity)
        {
            return new
            {
                entity?.TimeStamp,
                entity?.StarSystem
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
        [Route("{user}/{gameVersion}/Location")]
        public IActionResult PostLocation(string user, string gameVersion, [FromBody]LocationStateEntity location)
        {
            location.GameContext = new GameContext(gameVersion, user);

            _locationRepoz.Add(location);

            Log.Info(JsonConvert.SerializeObject(location));

            return Ok(location);
        }
    }
}