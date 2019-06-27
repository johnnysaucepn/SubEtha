using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Howatworks.Matrix.Site.Controllers
{
    public class ShipController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipController));

        private readonly IStateEntityRepository<ShipStateEntity> _shipRepoz;

        public ShipController(IStateEntityRepository<ShipStateEntity> shipRepoz)
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
        public IActionResult PostShip(string user, string gameVersion, [FromBody]ShipStateEntity ship)
        {
            ship.GameContext = new GameContext(gameVersion, user);

            _shipRepoz.Add(ship);

            Log.Info(JsonConvert.SerializeObject(ship));

            return Ok(ship);
        }
    }
}