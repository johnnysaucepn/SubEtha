using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Howatworks.Matrix.Core
{
    public static class TokenGenerator
    {
        public static readonly string Secret = "your secret goes here";

        public static string GenerateToken(string username, string commanderName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, username),
                new Claim("CommanderName", commanderName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: "ZZZ",
                //audience: "ZZZ",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(90),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }
    }
}
