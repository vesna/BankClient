using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankClientgPRCService.Securities
{
    public class JwtConfiguration
    {
        public string Key { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }

        internal SymmetricSecurityKey GetSingingKey() => new(Encoding.UTF8.GetBytes(Key));
    }
}
