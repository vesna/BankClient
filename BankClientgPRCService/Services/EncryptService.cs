using BankClientgPRCService.Services.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace BankClientgPRCService.Services
{
    public class EncryptService : IEncryptService
    {
        public string GenerateSalt() => Guid.NewGuid().ToString();

        public string HashPassword(string password, string salt)
        {
            var result = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 512 / 8
                );

            return Encoding.UTF8.GetString(result);
        }
    }
}
