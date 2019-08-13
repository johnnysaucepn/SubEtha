using Howatworks.Matrix.Core.Entities;
using Howatworks.Matrix.Core.Repositories;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Howatworks.Matrix.Site.Api
{
    [Route("Api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public IActionResult PostSession(string user, string gameVersion, [FromBody]SessionState session)
        {
            var sessionEntity = ToEntity(session, user, gameVersion);
            _sessionRepoz.Add(sessionEntity);

            Log.Info(JsonConvert.SerializeObject(sessionEntity));

            return Ok();
        }

        private static SessionStateEntity ToEntity(ISessionState session, string cmdrName, string version)
        {
            return new SessionStateEntity
            {
                CommanderName = cmdrName,
                GameVersion = version,
                TimeStamp = session.TimeStamp,

                Build = session.Build,
                GameMode = session.GameMode,
                Group = session.Group
            };

        }
    }
}