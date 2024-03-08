using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSigningKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var _claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(_claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = _creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var _tokenHandler = new JwtSecurityTokenHandler();

            var _token = _tokenHandler.CreateToken(_tokenDescriptor);

            return _tokenHandler.WriteToken(_token);
        }
    }
}