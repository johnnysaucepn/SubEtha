using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howatworks.Matrix.Site.Api
{
    //https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/

    [Route("Api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [Route("/token")]
        [HttpPost]
        public IActionResult Create(string username, string password)
        {
            if (IsValidUserAndPasswordCombination(username, password))
                return new ObjectResult(GenerateToken(username));
            return BadRequest();
        }

        private JwtSecurityToken GenerateToken(string username)
        {
            throw new NotImplementedException();
        }

        private bool IsValidUserAndPasswordCombination(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}