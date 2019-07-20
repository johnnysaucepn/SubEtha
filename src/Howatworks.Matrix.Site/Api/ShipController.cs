using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    public class ShipController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipController));

        private readonly IShipEntityRepository _shipRepoz;

        public ShipController(IShipEntityRepository shipRepoz)
        {
            _shipRepoz = shipRepoz;
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Ship")]
        public IActionResult GetShip(string user, string gameVersion)
        {
            return Ok(_shipRepoz.GetMostRecent(user, gameVersion));
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Ship/History")]
        public IActionResult GetShipHistory(string user, string gameVersion)
        {
            return Ok(_shipRepoz.GetRange(user, gameVersion, 0, int.MaxValue));
        }

        [HttpPost]
        [Route("{user}/{gameVersion}/Ship")]
        public IActionResult PostShip(string user, string gameVersion, [FromBody]ShipState ship)
        {
            var shipEntity = ToEntity(ship, user, gameVersion);

            _shipRepoz.Add(shipEntity);

            Log.Info(JsonConvert.SerializeObject(ship));

            return Ok();
        }

        private static ShipStateEntity ToEntity(IShipState ship, string cmdrName, string version)
        {
            return new ShipStateEntity
            {
                CommanderName = cmdrName,
                GameVersion = version,
                TimeStamp = ship.TimeStamp,

                ShipId = ship.ShipId,
                Type = ship.Type,
                Ident = ship.Ident,
                Name = ship.Name,
                HullIntegrity = ship.HullIntegrity,
                ShieldsUp = ship.ShieldsUp
            };

        }
    }
}