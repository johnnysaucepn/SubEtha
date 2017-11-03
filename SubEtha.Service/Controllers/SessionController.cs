using Newtonsoft.Json;
using log4net;
using Microsoft.AspNetCore.Mvc;
using SubEtha.Core.Entities;
using SubEtha.Core.Repositories;
using SubEtha.Domain;

namespace SubEtha.Service.Controllers
{
    public class SessionController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionController));

        private readonly ISessionEntityRepository _sessionRepoz;

        public SessionController(ISessionEntityRepository sessionRepoz)
        {
            _sessionRepoz = sessionRepoz;
        }

        [HttpGet]
        [Route("Users")]
        public IActionResult GetUsers()
        {
            return Ok(_sessionRepoz.EnumerateUsers());
        }

        [HttpGet]
        [Route("{user}/Versions")]
        public IActionResult GetVersions(string user)
        {
            return Ok(_sessionRepoz.EnumerateGameVersions(user));
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Session")]
        public IActionResult GetSession(string user, string gameVersion)
        {
            return Ok(_sessionRepoz.GetMostRecent(user, gameVersion));
        }

        [HttpGet]
        [Route("{user}/{gameVersion}/Session/History")]
        public IActionResult GetSessionHistory(string user, string gameVersion)
        {
            return Ok(_sessionRepoz.GetRange(user, gameVersion, 0, int.MaxValue));
        }

        [HttpPost]
        [Route("{user}/{gameVersion}/Session")]
        public IActionResult PostSession(string user, string gameVersion, [FromBody]SessionStateEntity session)
        {
            session.GameContext = new GameContext(gameVersion, user);

            _sessionRepoz.Add(session);

            Log.Info(JsonConvert.SerializeObject(session));

            return Ok(session);
        }        
    }
}