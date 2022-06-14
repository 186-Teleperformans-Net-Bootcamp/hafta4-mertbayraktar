using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MertBayraktar.Social.Network.Api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace MertBayraktar.Social.Network.Api.Generators
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GetToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
           issuer: _configuration["JWT:Issuer"],
           audience: _configuration["JWT:Audience"],
           expires: DateTime.Now.AddHours(1),
           claims: userClaims,
           signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
           );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
