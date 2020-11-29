using System.Threading.Tasks;
using Howatworks.Matrix.Data;
using Howatworks.Matrix.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Api
{
    //https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/

    //[Route("Api/[controller]")]
    [Route("Api")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<MatrixIdentityUser> _userManager;

        public TokenController(UserManager<MatrixIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("Token")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]string username, [FromForm]string password)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            // Note: Forbid() attempts to allow ASP.NET to use its handling logic,
            // which may result in redirection to a page.
            if (user == null)
                return Unauthorized();

            var validUser = await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
            if (!validUser)
                return Unauthorized();

            return new ObjectResult(TokenGenerator.GenerateToken(user.UserName, user.CommanderName ?? ""));
        }
    }
}