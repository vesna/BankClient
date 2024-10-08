using BankClientgPRCService.Securities;
using BankClientgPRCService.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankClientgPRCService.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public TokenService(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }

        public string GenerateToken(string phone, string roleName)
        {
            var cred = new SigningCredentials(_jwtConfiguration.GetSingingKey(), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.MobilePhone, phone),
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
