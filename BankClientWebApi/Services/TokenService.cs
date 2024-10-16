using BankClientWebApi.Protos;
using BankClientWebApi.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankClientWebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string phone, string roleName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                    new Claim(ClaimTypes.MobilePhone, phone),
                    new Claim(ClaimTypes.Role, roleName)
                };

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(Sectoken);
        }

        public string GetRoleNameFromToken(string stream)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(stream))
            {
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;

                var role = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                return role;
            }
            return string.Empty;
        }

        public string GetPhoneFromToken(string stream)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(stream))
            {
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;

                var phone = tokenS.Claims.First(claim => claim.Type == ClaimTypes.MobilePhone).Value;

                return phone;
            }
            return string.Empty;
        }
    }
}
